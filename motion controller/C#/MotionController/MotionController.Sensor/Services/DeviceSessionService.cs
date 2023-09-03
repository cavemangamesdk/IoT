using Microsoft.Extensions.Logging;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;

namespace MotionController.Services;

public interface IDeviceSessionService : IService
{
    Task<IEnumerable<DeviceSession?>> GetDeviceSessionsAsync();
    Task<DeviceSession?> GetDeviceSessionAsync(Guid sessionId);
    Task<DeviceSession?> AddDeviceSessionAsync(Guid sessionId);
    Task<DeviceSession?> GetOrAddDeviceSessionAsync(Guid sessionId);
}

internal class DeviceSessionService : ServiceBase<DeviceSessionService>, IDeviceSessionService
{
    public DeviceSessionService(ILogger<DeviceSessionService> logger, IDeviceSessionRepository deviceSessionRepository)
        : base(logger)
    {
        DeviceSessionRepository = deviceSessionRepository;
    }

    private IDeviceSessionRepository DeviceSessionRepository { get; }

    public async Task<IEnumerable<DeviceSession?>> GetDeviceSessionsAsync()
    {
        return await DeviceSessionRepository.GetAsync();
    }

    public async Task<DeviceSession?> GetDeviceSessionAsync(Guid sessionId)
    {
        return await DeviceSessionRepository.GetAsync(sessionId);
    }

    public async Task<DeviceSession?> AddDeviceSessionAsync(Guid sessionId)
    {
        var deviceSession = new DeviceSession
        {
            SessionId = sessionId
        };

        var created = await DeviceSessionRepository.AddAsync(deviceSession);
        if (!created)
        {
            return default;
        }
        return deviceSession;
    }

    public async Task<DeviceSession?> GetOrAddDeviceSessionAsync(Guid sessionId)
    {
        var deviceSession = await GetDeviceSessionAsync(sessionId);
        if (deviceSession?.Equals(default) ?? true)
        {
            deviceSession = await AddDeviceSessionAsync(sessionId);
        }
        return deviceSession;
    }
}
