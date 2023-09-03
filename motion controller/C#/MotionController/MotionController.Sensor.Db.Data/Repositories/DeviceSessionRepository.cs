using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionRepository : IRepository<DeviceSession, int>
{
    Task<DeviceSession?> GetAsync(Guid sessionId);
}

internal class DeviceSessionRepository : DbRepositoryBase<DeviceSession, int>, IDeviceSessionRepository
{
    public DeviceSessionRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<DeviceSession?> GetAsync(Guid sessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSession>()} WHERE ( [SessionId] = @sessionId )";

        return await QuerySingleOrDefaultAsync(query, new { @sessionId });
    }

    public override Task<bool> AddAsync(DeviceSession model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}
