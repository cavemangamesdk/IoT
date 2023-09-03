using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public sealed class DeviceAccelerometerDataBase
{
    [JsonProperty("roll")]
    public float Roll { get; set; }

    [JsonProperty("pitch")]
    public float Pitch { get; set; }

    [JsonProperty("yaw")]
    public float Yaw { get; set; }

    [JsonProperty("x_raw")]
    public float XRaw { get; set; }

    [JsonProperty("y_raw")]
    public float YRaw { get; set; }

    [JsonProperty("z_raw")]
    public float ZRaw { get; set; }
}

public sealed class DeviceAccelerometerData : DeviceDataBase<DeviceAccelerometerDataBase>
{
}
