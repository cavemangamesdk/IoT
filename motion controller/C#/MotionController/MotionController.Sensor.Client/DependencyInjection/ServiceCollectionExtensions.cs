using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace MotionController.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSensorClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SensorClientOptions>(configuration.GetSection(SensorClientOptions.SensorClient));

        services.AddHttpClient(typeof(SensorClientOptions).Assembly.FullName, (serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<SensorClientOptions>>();

            httpClient.BaseAddress = options.Value.BaseAddress;
            httpClient.DefaultRequestHeaders.Accept.AddApplicationJson();
        });

        return services;
    }

    private static void AddApplicationJson(this HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeader)
    {
        acceptHeader.Remove(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        acceptHeader.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }
}
