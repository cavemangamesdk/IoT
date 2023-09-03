using Microsoft.Extensions.Logging;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;

namespace MotionController.Services;

public interface IDeviceSessionAccelerometerService : IService
{
    Task<IEnumerable<DeviceSessionAccelerometer?>> GetDeviceSessionAccelerometerAsync(DeviceSession deviceSession);
    Task<bool> AddDeviceSessionAccelerometerAsync(DeviceSessionAccelerometer deviceSessionAccelerometer);
}

internal class DeviceSessionAccelerometerService : ServiceBase<DeviceSessionAccelerometerService>, IDeviceSessionAccelerometerService
{
    public DeviceSessionAccelerometerService(ILogger<DeviceSessionAccelerometerService> logger, IDeviceSessionAccelerometerRepository deviceSessionAccelerometerRepository)
        : base(logger)
    {
        DeviceSessionAccelerometerRepository = deviceSessionAccelerometerRepository;
    }

    private IDeviceSessionAccelerometerRepository DeviceSessionAccelerometerRepository { get; }

    public async Task<IEnumerable<DeviceSessionAccelerometer?>> GetDeviceSessionAccelerometerAsync(DeviceSession deviceSession)
    {
        return await DeviceSessionAccelerometerRepository.GetByDeviceSessionAsync(deviceSession.Id);
    }

    public async Task<bool> AddDeviceSessionAccelerometerAsync(DeviceSessionAccelerometer deviceSessionAccelerometer)
    {
        return await DeviceSessionAccelerometerRepository.AddAsync(deviceSessionAccelerometer);
    }
}
