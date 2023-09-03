using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MotionController.Extensions.Hosting;

public class HostedService<T> : BackgroundService where T : IBackgroundService
{
    protected ILogger<HostedService<T>> Logger { get; }

    protected IServiceProvider ServiceProvider { get; }

    public HostedService(ILogger<HostedService<T>> logger, IServiceProvider serviceProvider)
    {
        Logger = logger;
        ServiceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await ExecuteCoreAsync(stoppingToken);
        }
        catch (TaskCanceledException ex)
        {
            Logger.LogWarning(ex, $"Hosted service for '${typeof(T).Name}' stopped execution of tasks.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Hosted service for '${typeof(T).Name}' failed during execution of tasks.");
            throw;
        }
    }

    protected virtual async Task ExecuteCoreAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();
        T service = scope.ServiceProvider.GetService<T>();
        if (service != null)
        {
            await service.ExecuteAsync(stoppingToken);
            return;
        }

        Logger.LogError($"Background service '${typeof(T).Name}' was not found. Might not be registered");
    }
}
