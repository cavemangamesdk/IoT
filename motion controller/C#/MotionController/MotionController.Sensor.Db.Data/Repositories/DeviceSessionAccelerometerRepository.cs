using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionAccelerometerRepository : IRepository<DeviceSessionAccelerometer, int>
{
    Task<IEnumerable<DeviceSessionAccelerometer?>> GetByDeviceSessionAsync(int deviceSessionId);
}

internal class DeviceSessionAccelerometerRepository : DbRepositoryBase<DeviceSessionAccelerometer, int>, IDeviceSessionAccelerometerRepository
{
    public DeviceSessionAccelerometerRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<IEnumerable<DeviceSessionAccelerometer?>> GetByDeviceSessionAsync(int deviceSessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSessionAccelerometer>()} WHERE ( [DeviceSession_Id] = @deviceSessionId )";

        return await QueryAsync(query, new { @deviceSessionId });
    }

    public override Task<bool> AddAsync(DeviceSessionAccelerometer model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}

