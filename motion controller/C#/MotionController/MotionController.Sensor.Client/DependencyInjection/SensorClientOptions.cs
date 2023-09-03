namespace MotionController.Extensions.DependencyInjection;

public class SensorClientOptions
{
    public const string SensorClient = "MotionController.Sensor.Client";

    public Uri? BaseAddress { get; set; }
}
