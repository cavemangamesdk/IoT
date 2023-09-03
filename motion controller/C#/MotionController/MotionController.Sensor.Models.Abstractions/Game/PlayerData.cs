using Newtonsoft.Json;

namespace MotionController.Sensor.Models.Game;

public class PlayerData
{
    [JsonProperty("Name")]
    public string? Name { get; set; }

    [JsonProperty("Lives")]
    public int Lives { get; set; }

    [JsonProperty("GameTime")]
    public string? GameTime { get; set; }
}
