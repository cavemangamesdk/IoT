using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MotionController.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMotion(this IServiceCollection services, IConfiguration configuration)
    {
        var motionSection = configuration.GetSection(MotionOptions.Motion);

        services.Configure<MotionOptions>(motionSection)
            .ConfigureSensorMQTTSettings(motionSection);

        return services;
    }
}