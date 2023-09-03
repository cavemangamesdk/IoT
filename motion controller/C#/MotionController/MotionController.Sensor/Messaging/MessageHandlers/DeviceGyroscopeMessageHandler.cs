using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.MQTT;
using MotionController.MQTT.Messages;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models;
using MotionController.Services;

namespace MotionController.Sensor.Messaging.MessageHandlers;

[MQTTTopic("sensehat/imu/gyroscope")]
internal sealed class DeviceGyroscopeMessageHandler : MessageHandlerBase<DeviceGyroscopeMessageHandler, DeviceGyroscopeData>
{
    public DeviceGyroscopeMessageHandler(ILogger<DeviceGyroscopeMessageHandler> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    protected override async Task HandleModelAsync(DeviceGyroscopeData model)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var deviceSessionService = scope.ServiceProvider.GetRequiredService<IDeviceSessionService>();

        var deviceSessionGyroscopeService = scope.ServiceProvider.GetRequiredService<IDeviceSessionGyroscopeService>();

        var deviceSession = await deviceSessionService.GetOrAddDeviceSessionAsync(model.SessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            Logger.LogError("Bullshit");
            throw new Exception("Bullshit");
        }

        var deviceSessionGyroscope = new DeviceSessionGyroscope
        {
            DeviceSessionId = deviceSession.Id,
            Roll = model?.Data?.Roll ?? default,
            Pitch = model?.Data?.Pitch ?? default,
            Yaw = model?.Data?.Yaw ?? default,
            XRaw = model?.Data?.XRaw ?? default,
            YRaw = model?.Data?.YRaw ?? default,
            ZRaw = model?.Data?.ZRaw ?? default,
            Timestamp = model?.Timestamp ?? default
        };

        await deviceSessionGyroscopeService.AddDeviceSessionGyroscopeAsync(deviceSessionGyroscope);

        unitOfWork.Complete();
    }
}
