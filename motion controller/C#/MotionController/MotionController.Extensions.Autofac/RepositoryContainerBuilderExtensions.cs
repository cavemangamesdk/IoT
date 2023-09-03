using Autofac;
using MotionController.Data.Providers;
using MotionController.Data.Repositories;
using System.Reflection;

namespace VictorKrogh.Extensions.Autofac;

public static class RepositoryContainerBuilderExtensions
{
    public static ContainerBuilder RegisterRepositories<TProvider>(this ContainerBuilder containerBuilder) where TProvider : class, IProvider
    {
        containerBuilder.RegisterRepositories(typeof(TProvider).Assembly);
        return containerBuilder;
    }

    public static ContainerBuilder RegisterRepositories(this ContainerBuilder containerBuilder, Assembly assembly)
    {
        containerBuilder.RegisterAssemblyTypes(assembly).AssignableTo<IRepository>().AsImplementedInterfaces();
        return containerBuilder;
    }
}
