using Microsoft.Extensions.Logging;

namespace MotionController.Extensions.Hosting;

public interface IBackgroundService : IDisposable
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

public abstract class BackgroundService<TService> : IBackgroundService
    where TService : IBackgroundService
{
    protected BackgroundService(ILogger<TService> logger)
    {
        Logger = logger;
    }

    protected ILogger<TService> Logger { get; }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation($"{typeof(TService).Name} started executing.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ExecuteLogicAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{typeof(TService).Name} failed to perform work.");
            }

            await DelayAsync(cancellationToken);
        }

        Logger.LogInformation($"{typeof(TService).Name} ended executing.");
    }

    protected abstract Task ExecuteLogicAsync(CancellationToken cancellationToken);

    protected virtual async Task DelayAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}