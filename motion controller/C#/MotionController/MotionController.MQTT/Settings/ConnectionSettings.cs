namespace MotionController.MQTT.Settings;

public sealed class ConnectionSettings
{
    public string? Hostname { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}
