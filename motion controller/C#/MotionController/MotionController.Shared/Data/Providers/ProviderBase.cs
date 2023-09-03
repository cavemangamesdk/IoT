using System.Data;

namespace MotionController.Data.Providers;

public interface IProvider : IDisposable
{
    IsolationLevel IsolationLevel { get; }

    void Commit();
    void Rollback();
}

public class ProviderBase : Disposable, IProvider
{
    public ProviderBase(IsolationLevel isolationLevel)
    {
        IsolationLevel = isolationLevel;
    }

    public IsolationLevel IsolationLevel { get; }

    public virtual void Commit()
    {
    }

    public virtual void Rollback()
    {
    }
}
