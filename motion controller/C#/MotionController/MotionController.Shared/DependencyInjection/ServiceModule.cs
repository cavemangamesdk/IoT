using Autofac;
using MotionController.Services;

namespace MotionController.DependencyInjection;

internal class ServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(ThisAssembly)
            .AssignableTo<IService>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}

public class ServiceModule<T> : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(T).Assembly)
            .AssignableTo<IService>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}

