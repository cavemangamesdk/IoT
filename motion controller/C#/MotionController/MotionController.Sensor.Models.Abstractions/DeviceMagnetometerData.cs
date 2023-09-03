using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public sealed class DeviceMagnetometerDataBase
{
    [JsonProperty("roll")]
    public float North { get; set; }

    [JsonProperty("x_raw")]
    public float XRaw { get; set; }

    [JsonProperty("y_raw")]
    public float YRaw { get; set; }

    [JsonProperty("z_raw")]
    public float ZRaw { get; set; }
}

public sealed class DeviceMagnetometerData : DeviceDataBase<DeviceMagnetometerDataBase>
{
}
