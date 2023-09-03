using Newtonsoft.Json;

namespace MotionController.Sensor.Models.Game;

public class Vector2
{
    [JsonProperty("x")]
    public float X { get; set; }

    [JsonProperty("y")]
    public float Y { get; set; }
}