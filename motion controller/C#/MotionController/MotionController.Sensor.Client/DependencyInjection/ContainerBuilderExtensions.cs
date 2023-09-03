using Autofac;
using MotionController.Sensor.Client;

namespace MotionController.Extensions.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterSensorClient(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterAssemblyTypes(typeof(SensorClientOptions).Assembly)
            .Where(t => t.IsAssignableTo<IClient>() && !t.IsAbstract)
            .FindConstructorsWith(t => new[]
            {
                            t.GetConstructor(new []{ typeof(HttpClient) })
            })
            .WithParameter((pi, cc) => pi.ParameterType == typeof(HttpClient), (parameterInfo, componentContext) =>
            {
                var httpClientFactory = componentContext.Resolve<IHttpClientFactory>();

                return httpClientFactory.CreateClient(typeof(SensorClientOptions).Assembly.FullName);
            })
            .AsImplementedInterfaces();

        return containerBuilder;
    }
}
