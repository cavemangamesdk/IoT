using System.Data;

namespace MotionController.Data;

public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    public UnitOfWorkFactory(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected IServiceProvider ServiceProvider { get; }

    public IUnitOfWork CreateUnitOfWork(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return new UnitOfWork(ServiceProvider, isolationLevel);
    }
}
