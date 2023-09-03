using System.Data;
using System.Reflection;

namespace MotionController.Data.Providers.Database.SqlClient;

public class SqlClientProviderFactory<TProvider> : IProviderFactory<TProvider>
    where TProvider : ISqlClientDbProvider
{
    public SqlClientProviderFactory(SqlClientProviderSettings sqlClientProviderSettings)
    {
        SqlClientProviderSettings = sqlClientProviderSettings;
    }

    private SqlClientProviderSettings SqlClientProviderSettings { get; }

    public TProvider CreateProvider(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var type = typeof(TProvider);

        var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(IsolationLevel), typeof(SqlClientProviderSettings) }, null);
        if (ctor == null)
        {
            throw new NotImplementedException("Constructor not implemented.");
        }

        var instance = ctor.Invoke(new object[] { isolationLevel, SqlClientProviderSettings });

        return (TProvider)instance;
    }
}
