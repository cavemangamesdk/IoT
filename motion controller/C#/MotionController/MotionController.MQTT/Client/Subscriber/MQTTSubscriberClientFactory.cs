using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotionController.MQTT.Settings;

namespace MotionController.MQTT.Client.Subscriber;

public interface IMQTTSubscriberClientFactory
{
    IMQTTSubscriberClient CreateSubscriberClient<TMQTTSettings>() where TMQTTSettings : MQTTSubscriberClientSettingsBase;
}

internal sealed class MQTTSubscriberClientFactory : IMQTTSubscriberClientFactory
{
    public MQTTSubscriberClientFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public IMQTTSubscriberClient CreateSubscriberClient<TMQTTSettings>()
        where TMQTTSettings : MQTTSubscriberClientSettingsBase
    {
        var logger = ServiceProvider.GetRequiredService<ILogger<MQTTSubscriberClient>>();
        var mqttClient = ServiceProvider.GetRequiredService<MQTTnet.Client.IMqttClient>();
        var mqttClientOptions = ServiceProvider.GetRequiredService<IOptions<TMQTTSettings>>();

        return new MQTTSubscriberClient(logger, mqttClient, mqttClientOptions.Value);
    }
}