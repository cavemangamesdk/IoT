using Newtonsoft.Json;

namespace MotionController.Sensor.Models;

public class DeviceDataBase : ISessionIdentifier
{
    [JsonProperty("session_id")]
    public Guid SessionId { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}

public class DeviceDataBase<TData> : DeviceDataBase
{
    [JsonProperty("data")]
    public TData? Data { get; set; }
}
