using System.Data.SqlClient;

namespace MotionController.Data.Providers.Database.SqlClient;

public class SqlClientProviderSettings : IDbProviderSettings
{
    public string? ConnectionString { get; set; }
    public SqlCredential? Credentials { get; set; }
}
