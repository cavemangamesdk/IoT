using Autofac;
using MotionController.BackgroundServices;
using MotionController.DependencyInjection;
using MotionController.Extensions.Autofac;
using MotionController.Sensor.Db.Data.Providers;
using MotionController.Sensor.Messaging;
using MotionController.Sensor.MQTT;

namespace MotionController.Extensions.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterMotionController(this ContainerBuilder containerBuilder, MotionOptions? motionOptions)
    {
        if (motionOptions == null)
        {
            throw new Exception("");
        }

        containerBuilder.RegisterType<MessageHandlerResolver>()
            .As<IMessageHandlerResolver>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterSqlClientProvider<MotionProvider>(motionOptions.SqlClientProviderSettings);

        containerBuilder.RegisterModule<ServiceModule<MotionOptions>>();

        containerBuilder.RegisterMQTT()
            .WithConnection<SensorMQTTSettings>(Guid.NewGuid().ToString())
            .WithMessageHandlers(typeof(MotionOptions).Assembly);

        return containerBuilder;
    }

    public static ContainerBuilder WithMQTTClientBackgroundService(this ContainerBuilder containerBuilder)
    {
        return containerBuilder.RegisterBackgroundService<MQTTClientBackgroundService>();
    }
}
