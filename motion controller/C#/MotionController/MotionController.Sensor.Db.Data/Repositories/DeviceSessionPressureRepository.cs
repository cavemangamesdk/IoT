using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IDeviceSessionPressureRepository : IRepository<DeviceSessionPressure, int>
{
    Task<IEnumerable<DeviceSessionPressure?>> GetAsync(int deviceSessionId);
}

internal class DeviceSessionPressureRepository : DbRepositoryBase<DeviceSessionPressure, int>, IDeviceSessionPressureRepository
{
    public DeviceSessionPressureRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    async Task<IEnumerable<DeviceSessionPressure?>> IDeviceSessionPressureRepository.GetAsync(int deviceSessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<DeviceSessionPressure>()} WHERE ( [DeviceSession_Id] = @deviceSessionId )";

        return await QueryAsync(query, new { deviceSessionId });
    }

    public override Task<bool> AddAsync(DeviceSessionPressure model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}
