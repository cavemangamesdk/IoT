using Microsoft.Extensions.DependencyInjection;
using MotionController.Data.Providers;
using MotionController.Data.Repositories;
using System.Data;

namespace MotionController.Data;

public interface IUnitOfWork : IProviderFactory, IDisposable
{
    IsolationLevel IsolationLevel { get; }

    bool IsCompleted { get; }

    void Complete();

    TRepository GetRepository<TRepository>() where TRepository : IRepository;
}

internal class UnitOfWork : Disposable, IUnitOfWork
{
    public UnitOfWork(IServiceProvider serviceProvider, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        IsolationLevel = isolationLevel;

        InnerProviders = new List<IProvider>();
    }

    protected IServiceProvider ServiceProvider { get; }
    public IsolationLevel IsolationLevel { get; }

    public bool IsCompleted { get; set; }

    private IList<IProvider> InnerProviders { get; set; }

    public void Complete()
    {
        foreach (var provider in InnerProviders)
        {
            provider.Commit();
        }

        IsCompleted = true;
    }

    public TProvider CreateProvider<TProvider>(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where TProvider : IProvider
    {
        var providerFactory = ServiceProvider.GetService<IProviderFactory<TProvider>>();

        var provider = providerFactory.CreateProvider(isolationLevel);

        InnerProviders.Add(provider);

        return provider;
    }

    public TRepository GetRepository<TRepository>()
        where TRepository : IRepository
    {
        return ServiceProvider.GetRequiredService<TRepository>();
    }

    protected override void DisposeManagedState()
    {
        InnerProviders.Clear();
    }
}
