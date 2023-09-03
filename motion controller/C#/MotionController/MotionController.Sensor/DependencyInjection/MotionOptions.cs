using MotionController.Data.Providers.Database.SqlClient;
using MotionController.Sensor.MQTT;

namespace MotionController.Extensions.DependencyInjection;

public class MotionOptions
{
    public const string Motion = "MotionController";

    public SqlClientProviderSettings? SqlClientProviderSettings { get; set; }
    public SensorMQTTSettings? SensorMQTTSettings { get; set; }
}
