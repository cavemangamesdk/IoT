using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.MQTT;
using MotionController.MQTT.Messages;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models;
using MotionController.Services;

namespace MotionController.Sensor.Messaging.MessageHandlers;

[MQTTTopic("sensehat/imu/orientation")]
internal sealed class DeviceOrientationMessageHandler : MessageHandlerBase<DeviceOrientationMessageHandler, DeviceOrientationData>
{
    public DeviceOrientationMessageHandler(ILogger<DeviceOrientationMessageHandler> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    protected override async Task HandleModelAsync(DeviceOrientationData model)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var deviceSessionService = scope.ServiceProvider.GetRequiredService<IDeviceSessionService>();

        var deviceSessionOrientationService = scope.ServiceProvider.GetRequiredService<IDeviceSessionOrientationService>();

        var deviceSession = await deviceSessionService.GetOrAddDeviceSessionAsync(model.SessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            Logger.LogError("Bullshit");
            throw new Exception("Bullshit");
        }

        var deviceSessionOrientation = new DeviceSessionOrientation
        {
            DeviceSessionId = deviceSession.Id,
            RollDegrees = model?.Data?.RollDegrees ?? default,
            PitchDegrees = model?.Data?.PitchDegrees ?? default,
            YawDegrees = model?.Data?.YawDegrees ?? default,
            RollRadians = model?.Data?.RollRadians ?? default,
            PitchRadians = model?.Data?.PitchRadians ?? default,
            YawRadians = model?.Data?.YawRadians ?? default,
            Timestamp = model?.Timestamp ?? default
        };

        await deviceSessionOrientationService.AddDeviceSessionOrientationAsync(deviceSessionOrientation);

        unitOfWork.Complete();
    }
}
