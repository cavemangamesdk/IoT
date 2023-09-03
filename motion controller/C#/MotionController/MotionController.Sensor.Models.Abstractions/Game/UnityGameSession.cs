using Newtonsoft.Json;

namespace MotionController.Sensor.Models.Game;

public class UnityGameSession
{
    [JsonProperty("Guid")]
    public Guid Guid { get; set; }

    [JsonProperty("PlayerData")]
    public PlayerData? PlayerData { get; set; }

    [JsonProperty("GameData")]
    public GameData[] GameData { get; set; } = Array.Empty<GameData>();
}
