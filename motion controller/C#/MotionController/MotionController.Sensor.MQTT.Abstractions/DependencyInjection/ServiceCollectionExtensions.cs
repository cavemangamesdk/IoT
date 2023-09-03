using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotionController.Sensor.MQTT;

namespace MotionController.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSensorMQTTSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SensorMQTTSettings>(configuration.GetSection(SensorMQTTSettings.SensorMQTT));

        return services;
    }
}