using Autofac;
using Autofac.Core;
using MotionController.Data;
using System.Data;

namespace VictorKrogh.Extensions.Autofac;

public static class UnitOfWorkContainerBuilderExtensions
{
    public static ContainerBuilder RegisterUnitOfWorkFactory(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<UnitOfWorkFactory>()
            .As<IUnitOfWorkFactory>()
            .InstancePerLifetimeScope();

        containerBuilder.Register(UnitOfWorkFactory)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        return containerBuilder;
    }

    private static IUnitOfWork UnitOfWorkFactory(IComponentContext componentContext, IEnumerable<Parameter> parameters)
    {
        var factory = componentContext.Resolve<IUnitOfWorkFactory>();

        if (!parameters.Any())
        {
            return factory.CreateUnitOfWork();
        }
        else
        {
            var isolationLevel = parameters.TypedAs<IsolationLevel>();
            return factory.CreateUnitOfWork(isolationLevel);
        }
    }
}
