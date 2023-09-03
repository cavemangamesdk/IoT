using Autofac;
using Microsoft.Extensions.Hosting;
using MotionController.Extensions.Hosting;

namespace MotionController.Extensions.Autofac;

public static class BackgroundServiceContainerBuilderExtensions
{
    public static ContainerBuilder RegisterBackgroundService<T>(this ContainerBuilder containerBuilder)
    {
        return containerBuilder.RegisterBackgroundService(typeof(T));
    }

    public static ContainerBuilder RegisterBackgroundService(this ContainerBuilder containerBuilder, Type backgroundServiceType)
    {
        var genericType = typeof(HostedService<>).MakeGenericType(backgroundServiceType);

        containerBuilder.RegisterType(genericType)
            .As<IHostedService>()
            .SingleInstance();

        containerBuilder.RegisterType(backgroundServiceType)
            .AsSelf()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        return containerBuilder;
    }
}