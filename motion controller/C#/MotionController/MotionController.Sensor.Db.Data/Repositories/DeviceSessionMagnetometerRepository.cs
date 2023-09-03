using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionMagnetometerRepository : IRepository<DeviceSessionMagnetometer, int>
{
    Task<IEnumerable<DeviceSessionMagnetometer?>> GetByDeviceSessionAsync(int deviceSessionId);
}

internal class DeviceSessionMagnetometerRepository : DbRepositoryBase<DeviceSessionMagnetometer, int>, IDeviceSessionMagnetometerRepository
{
    public DeviceSessionMagnetometerRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<IEnumerable<DeviceSessionMagnetometer?>> GetByDeviceSessionAsync(int deviceSessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSessionMagnetometer>()} WHERE ( [DeviceSession_Id] = @deviceSessionId )";

        return await QueryAsync(query, new { @deviceSessionId });
    }

    public override Task<bool> AddAsync(DeviceSessionMagnetometer model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}

