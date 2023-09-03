namespace MotionController.MQTT.Client;

public interface IMQTTClient : IDisposable, IAsyncDisposable
{
    Func<ArraySegment<byte>, string, Task>? ReceivedMessageAsync { get; set; }
}
