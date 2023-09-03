using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.Extensions.Hosting;
using MotionController.MQTT.Client.Subscriber;
using MotionController.Sensor.Messaging;
using MotionController.Sensor.MQTT;
using System.Text;

namespace MotionController.BackgroundServices;

public interface IMQTTClientBackgroundService : IBackgroundService
{
}

internal class MQTTClientBackgroundService : BackgroundService<MQTTClientBackgroundService>, IMQTTClientBackgroundService
{
    public MQTTClientBackgroundService(ILogger<MQTTClientBackgroundService> logger, IServiceProvider serviceProvider)
        : base(logger)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    protected override async Task ExecuteLogicAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = ServiceProvider.CreateScope();

            var mqttSubscriberClientFactory = scope.ServiceProvider.GetRequiredService<IMQTTSubscriberClientFactory>();

            using var subscriberClient = mqttSubscriberClientFactory.CreateSubscriberClient<SensorMQTTSettings>();

            subscriberClient.ReceivedMessageAsync += OnReceivedMessageAsync;

            await subscriberClient.SubscribeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task OnReceivedMessageAsync(ArraySegment<byte> payload, string topic)
    {
        try
        {
            using var scope = ServiceProvider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var messageHandlerResolver = scope.ServiceProvider.GetRequiredService<IMessageHandlerResolver>();

            var utf8Message = Encoding.UTF8.GetString(payload);

            var messageHandler = messageHandlerResolver.Resolve(topic);
            if (messageHandler == default)
            {
                Logger.LogWarning($"No message handler for the given topic {topic}");
                return;
            }

            await messageHandler.HandleAsync(utf8Message);

            unitOfWork.Complete();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
