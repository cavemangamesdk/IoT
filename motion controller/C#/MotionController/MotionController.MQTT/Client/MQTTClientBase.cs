using Microsoft.Extensions.Logging;
using MQTTnet.Client;

namespace MotionController.MQTT.Client;

internal abstract class MQTTClientBase : IDisposable, IAsyncDisposable
{
    public MQTTClientBase(ILogger<IMQTTClient> logger, IMqttClient mqttClient)
    {
        Logger = logger;
        MqttClient = mqttClient;
    }

    protected ILogger<IMQTTClient> Logger { get; }
    protected IMqttClient MqttClient { get; }

    protected virtual void Dispose(bool disposing)
    {
        MqttClient.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await MqttClient.DisconnectAsync();

        Dispose();
    }
}