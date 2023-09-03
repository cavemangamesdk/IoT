namespace MotionController.Data.Providers.Database;

public interface IDbProviderSettings
{
    string? ConnectionString { get; set; }
}
