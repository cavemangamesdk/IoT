using MotionController.Data.Providers;

namespace MotionController.Data.Repositories;

public abstract class RepositoryBase<TProvider> : IRepository
    where TProvider : IProvider
{
    protected RepositoryBase(TProvider provider)
    {
        Provider = provider;
    }

    protected TProvider Provider { get; }
}