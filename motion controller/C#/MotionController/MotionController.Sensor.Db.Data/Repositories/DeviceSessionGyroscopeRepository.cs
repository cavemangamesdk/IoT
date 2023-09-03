using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionGyroscopeRepository : IRepository<DeviceSessionGyroscope, int>
{
    Task<IEnumerable<DeviceSessionGyroscope?>> GetByDeviceSessionAsync(int deviceSessionId);
}

internal class DeviceSessionGyroscopeRepository : DbRepositoryBase<DeviceSessionGyroscope, int>, IDeviceSessionGyroscopeRepository
{
    public DeviceSessionGyroscopeRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<IEnumerable<DeviceSessionGyroscope?>> GetByDeviceSessionAsync(int deviceSessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSessionGyroscope>()} WHERE ( [DeviceSession_Id] = @deviceSessionId )";

        return await QueryAsync(query, new { @deviceSessionId });
    }

    public override Task<bool> AddAsync(DeviceSessionGyroscope model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}

