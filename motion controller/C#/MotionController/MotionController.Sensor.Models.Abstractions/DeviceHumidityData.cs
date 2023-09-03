using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public sealed class DeviceHumidityDataBase
{
    [JsonProperty("humidity")]
    public float Humidity { get; set; }

    [JsonProperty("temperature")]
    public float Temperature { get; set; }
}

public sealed class DeviceHumidityData : DeviceDataBase
{
    [JsonProperty("data")]
    public DeviceHumidityDataBase? Data { get; set; }
}
