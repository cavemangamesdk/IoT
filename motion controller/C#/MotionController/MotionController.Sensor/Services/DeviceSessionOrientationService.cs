using Microsoft.Extensions.Logging;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;

namespace MotionController.Services;

public interface IDeviceSessionOrientationService : IService
{
    Task<IEnumerable<DeviceSessionOrientation?>> GetDeviceSessionOrientationAsync(DeviceSession deviceSession);
    Task<bool> AddDeviceSessionOrientationAsync(DeviceSessionOrientation deviceSessionOrientation);
}

internal class DeviceSessionOrientationService : ServiceBase<DeviceSessionOrientationService>, IDeviceSessionOrientationService
{
    public DeviceSessionOrientationService(ILogger<DeviceSessionOrientationService> logger, IDeviceSessionOrientationRepository deviceSessionOrientationRepository)
        : base(logger)
    {
        DeviceSessionOrientationRepository = deviceSessionOrientationRepository;
    }

    private IDeviceSessionOrientationRepository DeviceSessionOrientationRepository { get; }

    public async Task<IEnumerable<DeviceSessionOrientation?>> GetDeviceSessionOrientationAsync(DeviceSession deviceSession)
    {
        return await DeviceSessionOrientationRepository.GetByDeviceSessionAsync(deviceSession.Id);
    }

    public async Task<bool> AddDeviceSessionOrientationAsync(DeviceSessionOrientation deviceSessionOrientation)
    {
        return await DeviceSessionOrientationRepository.AddAsync(deviceSessionOrientation);
    }
}
