using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public sealed class DevicePressureDataBase
{
    [JsonProperty("pressure")]
    public float Pressure { get; set; }

    [JsonProperty("temperature")]
    public float Temperature { get; set; }
}

public sealed class DevicePressureData : DeviceDataBase
{
    [JsonProperty("data")]
    public DevicePressureDataBase? Data { get; set; }
}

