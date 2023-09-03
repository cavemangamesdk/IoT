using Newtonsoft.Json;

namespace MotionController.Sensor.Models.Game;

public class GameData
{
    [JsonProperty("BallPosition")]
    public Vector3? BallPosition { get; set; }

    [JsonProperty("BoardRotation")]
    public Vector3? BoardRotation { get; set; }

    [JsonProperty("InputData")]
    public Vector2? InputData { get; set; }
}
