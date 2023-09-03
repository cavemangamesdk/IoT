using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.MQTT;
using MotionController.MQTT.Messages;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models;
using MotionController.Services;

namespace MotionController.Sensor.Messaging.MessageHandlers;

[MQTTTopic("sensehat/imu/magnetometer")]
internal sealed class DeviceMagnetometerMessageHandler : MessageHandlerBase<DeviceMagnetometerMessageHandler, DeviceMagnetometerData>
{
    public DeviceMagnetometerMessageHandler(ILogger<DeviceMagnetometerMessageHandler> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    protected override async Task HandleModelAsync(DeviceMagnetometerData model)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var deviceSessionService = scope.ServiceProvider.GetRequiredService<IDeviceSessionService>();

        var deviceSessionMagnetometerService = scope.ServiceProvider.GetRequiredService<IDeviceSessionMagnetometerService>();

        var deviceSession = await deviceSessionService.GetOrAddDeviceSessionAsync(model.SessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            Logger.LogError("Bullshit");
            throw new Exception("Bullshit");
        }

        var deviceSessionMagnetometer = new DeviceSessionMagnetometer
        {
            DeviceSessionId = deviceSession.Id,
            North = model?.Data?.North ?? default,
            XRaw = model?.Data?.XRaw ?? default,
            YRaw = model?.Data?.YRaw ?? default,
            ZRaw = model?.Data?.ZRaw ?? default,
            Timestamp = model?.Timestamp ?? default
        };

        await deviceSessionMagnetometerService.AddDeviceSessionMagnetometerAsync(deviceSessionMagnetometer);

        unitOfWork.Complete();
    }
}
