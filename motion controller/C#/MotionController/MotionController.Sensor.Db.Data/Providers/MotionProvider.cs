using MotionController.Data.Providers.Database.SqlClient;
using System.Data;

namespace MotionController.Sensor.Db.Data.Providers;

public interface IMotionProvider : ISqlClientDbProvider
{
}

internal class MotionProvider : SqlClientDbProviderBase, IMotionProvider
{
    public MotionProvider(IsolationLevel isolationLevel, SqlClientProviderSettings sqlClientProviderSettings)
        : base(isolationLevel, sqlClientProviderSettings)
    {
    }
}
