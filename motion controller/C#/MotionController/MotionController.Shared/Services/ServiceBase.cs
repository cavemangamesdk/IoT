using Microsoft.Extensions.Logging;

namespace MotionController.Services;

public interface IService
{
}

public abstract class ServiceBase<TService>
{
    public ServiceBase(ILogger<TService> logger)
    {
        Logger = logger;
    }

    protected ILogger<TService> Logger { get; }
}
