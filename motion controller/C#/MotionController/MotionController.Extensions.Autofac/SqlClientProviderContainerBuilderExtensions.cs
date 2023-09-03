using MotionController.Data.Providers;
using MotionController.Data.Providers.Database.SqlClient;
using MotionController.Data.Repositories;
using System.Data;
using System.Reflection;

namespace Autofac;

public static class SqlClientProviderContainerBuilderExtensions
{
    public static ContainerBuilder RegisterSqlClientProvider<TProvider>(this ContainerBuilder containerBuilder, Action<SqlClientProviderSettings> configure)
        where TProvider : class, ISqlClientDbProvider
    {
        var sqlClientProviderSettings = new SqlClientProviderSettings();

        configure(sqlClientProviderSettings);

        return containerBuilder.RegisterSqlClientProvider<TProvider>(sqlClientProviderSettings);
    }

    public static ContainerBuilder RegisterSqlClientProvider<TProvider>(this ContainerBuilder containerBuilder, SqlClientProviderSettings? sqlClientProviderSettings)
        where TProvider : class, ISqlClientDbProvider
    {
        containerBuilder.RegisterProvider<TProvider>();

        containerBuilder.RegisterSqlClientProviderFactory<TProvider>(sqlClientProviderSettings);

        containerBuilder.RegisterRepositories<TProvider>();

        return containerBuilder;
    }

    private static void RegisterProvider<TProvider>(this ContainerBuilder containerBuilder)
        where TProvider : class, ISqlClientDbProvider
    {
        containerBuilder.Register((cc, p) =>
        {
            var providerFactory = cc.Resolve<IProviderFactory>();

            if (p.Any())
            {
                return providerFactory.CreateProvider<TProvider>(p.TypedAs<IsolationLevel>());
            }
            return providerFactory.CreateProvider<TProvider>();
        })
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }

    private static void RegisterSqlClientProviderFactory<TProvider>(this ContainerBuilder containerBuilder, SqlClientProviderSettings sqlClientProviderSettings)
        where TProvider : class, ISqlClientDbProvider
    {
        containerBuilder.RegisterType<SqlClientProviderFactory<TProvider>>()
            .UsingConstructor(typeof(SqlClientProviderSettings))
            .WithParameter(TypedParameter.From(sqlClientProviderSettings))
            .As<IProviderFactory<TProvider>>()
            .InstancePerLifetimeScope();
    }

    private static void RegisterRepositories<TProvider>(this ContainerBuilder containerBuilder)
        where TProvider : class, ISqlClientDbProvider
    {
        containerBuilder.RegisterRepositories(typeof(TProvider).Assembly);
    }

    private static void RegisterRepositories(this ContainerBuilder containerBuilder, Assembly assembly)
    {
        containerBuilder.RegisterAssemblyTypes(assembly)
            .AssignableTo<IRepository>()
            .AsImplementedInterfaces();
    }
}
