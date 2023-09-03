using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.MQTT;
using MotionController.MQTT.Messages;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models;
using MotionController.Services;

namespace MotionController.Sensor.Messaging.MessageHandlers;

[MQTTTopic("sensehat/env/pressure")]
internal sealed class DevicePressureMessageHandler : MessageHandlerBase<DevicePressureMessageHandler, DevicePressureData>
{
    public DevicePressureMessageHandler(ILogger<DevicePressureMessageHandler> logger, IServiceProvider serviceProvider)
        : base(logger, serviceProvider)
    {
    }

    protected override async Task HandleModelAsync(DevicePressureData model)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var deviceSessionService = scope.ServiceProvider.GetRequiredService<IDeviceSessionService>();
        var deviceSessionPressureService = scope.ServiceProvider.GetRequiredService<IDeviceSessionPressureService>();

        var deviceSession = await deviceSessionService.GetOrAddDeviceSessionAsync(model.SessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            Logger.LogError("Bullshit");
            throw new Exception("Bullshit");
        }

        var deviceSessionPressure = new DeviceSessionPressure
        {
            DeviceSessionId = deviceSession.Id,
            TemperatureCelsius = model?.Data?.Temperature ?? default,
            PressureMillibars = model?.Data?.Pressure ?? default,
            Timestamp = model?.Timestamp ?? default
        };

        await deviceSessionPressureService.AddDeviceSessionPressureAsync(deviceSessionPressure);

        unitOfWork.Complete();
    }
}
