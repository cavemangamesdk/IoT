using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionOrientationRepository : IRepository<DeviceSessionOrientation, int>
{
    Task<IEnumerable<DeviceSessionOrientation?>> GetByDeviceSessionAsync(int deviceSessionId);
}

internal class DeviceSessionOrientationRepository : DbRepositoryBase<DeviceSessionOrientation, int>, IDeviceSessionOrientationRepository
{
    public DeviceSessionOrientationRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<IEnumerable<DeviceSessionOrientation?>> GetByDeviceSessionAsync(int deviceSessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSessionOrientation>()} WHERE ( [DeviceSession_Id] = @deviceSessionId )";

        return await QueryAsync(query, new { @deviceSessionId });
    }

    public override Task<bool> AddAsync(DeviceSessionOrientation model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}

