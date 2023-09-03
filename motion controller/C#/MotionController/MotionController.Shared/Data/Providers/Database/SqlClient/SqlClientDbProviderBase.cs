using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MotionController.Data.Providers.Database.SqlClient;

public interface ISqlClientDbProvider : IDbProvider
{
}

public class SqlClientDbProviderBase : DbProviderBase, ISqlClientDbProvider
{
    public SqlClientDbProviderBase(IsolationLevel isolationLevel, SqlClientProviderSettings sqlClientProviderSettings)
        : base(isolationLevel)
    {
        SqlClientProviderSettings = sqlClientProviderSettings;
    }

    private SqlClientProviderSettings SqlClientProviderSettings { get; }

    protected override IDbConnection CreateConnection()
    {
        var connectionBuilder = new SqlConnectionStringBuilder(SqlClientProviderSettings.ConnectionString);

        connectionBuilder.ApplicationName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? connectionBuilder.ApplicationName;

        return new SqlConnection(connectionBuilder.ToString(), SqlClientProviderSettings.Credentials);
    }
}
