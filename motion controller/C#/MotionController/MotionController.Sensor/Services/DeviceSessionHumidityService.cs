using Microsoft.Extensions.Logging;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;

namespace MotionController.Services;

public interface IDeviceSessionHumidityService : IService
{
    Task<IEnumerable<DeviceSessionHumidity?>> GetDeviceSessionHumidityAsync(DeviceSession deviceSession);
    Task<bool> AddDeviceSessionHumidityAsync(DeviceSessionHumidity deviceSessionHumidity);
}

internal class DeviceSessionHumidityService : ServiceBase<DeviceSessionHumidityService>, IDeviceSessionHumidityService
{
    public DeviceSessionHumidityService(ILogger<DeviceSessionHumidityService> logger, IDeviceSessionHumidityRepository deviceSessionHumidityRepository)
        : base(logger)
    {
        DeviceSessionHumidityRepository = deviceSessionHumidityRepository;
    }

    private IDeviceSessionHumidityRepository DeviceSessionHumidityRepository { get; }

    public async Task<IEnumerable<DeviceSessionHumidity?>> GetDeviceSessionHumidityAsync(DeviceSession deviceSession)
    {
        return await DeviceSessionHumidityRepository.GetByDeviceSessionAsync(deviceSession.Id);
    }

    public async Task<bool> AddDeviceSessionHumidityAsync(DeviceSessionHumidity deviceSessionHumidity)
    {
        return await DeviceSessionHumidityRepository.AddAsync(deviceSessionHumidity);
    }
}
