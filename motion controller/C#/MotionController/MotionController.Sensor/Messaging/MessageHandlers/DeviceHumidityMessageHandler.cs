using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.MQTT;
using MotionController.MQTT.Messages;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models;
using MotionController.Services;

namespace MotionController.Sensor.Messaging.MessageHandlers;

[MQTTTopic("sensehat/env/humidity")]
internal sealed class DeviceHumidityMessageHandler : MessageHandlerBase<DeviceHumidityMessageHandler, DeviceHumidityData>
{
    public DeviceHumidityMessageHandler(ILogger<DeviceHumidityMessageHandler> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    protected override async Task HandleModelAsync(DeviceHumidityData model)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var deviceSessionService = scope.ServiceProvider.GetRequiredService<IDeviceSessionService>();
        var deviceSessionHumidityService = scope.ServiceProvider.GetRequiredService<IDeviceSessionHumidityService>();

        var deviceSession = await deviceSessionService.GetOrAddDeviceSessionAsync(model.SessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            Logger.LogError("Bullshit");
            throw new Exception("Bullshit");
        }

        var deviceSessionHumidity = new DeviceSessionHumidity
        {
            DeviceSessionId = deviceSession.Id,
            TemperatureCelsius = model?.Data?.Temperature ?? default,
            HumidityPercentage = model?.Data?.Humidity ?? default,
            Timestamp = model?.Timestamp ?? default
        };

        await deviceSessionHumidityService.AddDeviceSessionHumidityAsync(deviceSessionHumidity);

        unitOfWork.Complete();
    }
}
