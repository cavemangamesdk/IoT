using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public sealed class DeviceOrientationDataBase
{
    [JsonProperty("roll_deg")]
    public float RollDegrees { get; set; }

    [JsonProperty("pitch_deg")]
    public float PitchDegrees { get; set; }

    [JsonProperty("yaw_deg")]
    public float YawDegrees { get; set; }

    [JsonProperty("roll_rad")]
    public float RollRadians { get; set; }

    [JsonProperty("pitch_rad")]
    public float PitchRadians { get; set; }

    [JsonProperty("yaw_rad")]
    public float YawRadians { get; set; }
}

public sealed class DeviceOrientationData : DeviceDataBase<DeviceOrientationDataBase>
{
}
