using Microsoft.Extensions.Logging;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;

namespace MotionController.Services;

public interface IDeviceSessionMagnetometerService : IService
{
    Task<IEnumerable<DeviceSessionMagnetometer?>> GetDeviceSessionMagnetometerAsync(DeviceSession deviceSession);
    Task<bool> AddDeviceSessionMagnetometerAsync(DeviceSessionMagnetometer deviceSessionMagnetometer);
}

internal class DeviceSessionMagnetometerService : ServiceBase<DeviceSessionMagnetometerService>, IDeviceSessionMagnetometerService
{
    public DeviceSessionMagnetometerService(ILogger<DeviceSessionMagnetometerService> logger, IDeviceSessionMagnetometerRepository deviceSessionMagnetometerRepository)
        : base(logger)
    {
        DeviceSessionMagnetometerRepository = deviceSessionMagnetometerRepository;
    }

    private IDeviceSessionMagnetometerRepository DeviceSessionMagnetometerRepository { get; }

    public async Task<IEnumerable<DeviceSessionMagnetometer?>> GetDeviceSessionMagnetometerAsync(DeviceSession deviceSession)
    {
        return await DeviceSessionMagnetometerRepository.GetByDeviceSessionAsync(deviceSession.Id);
    }

    public async Task<bool> AddDeviceSessionMagnetometerAsync(DeviceSessionMagnetometer deviceSessionMagnetometer)
    {
        return await DeviceSessionMagnetometerRepository.AddAsync(deviceSessionMagnetometer);
    }
}
