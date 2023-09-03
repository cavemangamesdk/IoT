using Microsoft.Extensions.Logging;
using MotionController.MQTT.Settings;
using MQTTnet.Client;

namespace MotionController.MQTT.Client.Subscriber;

public interface IMQTTSubscriberClient : IMQTTClient
{
    Task SubscribeAsync(CancellationToken cancellationToken);
}

internal class MQTTSubscriberClient : MQTTClientBase, IMQTTSubscriberClient
{
    public MQTTSubscriberClient(ILogger<MQTTSubscriberClient> logger, IMqttClient mqttClient, MQTTSubscriberClientSettingsBase mqttSubscriberClientSettingsBase)
        : base(logger, mqttClient)
    {
        MqttSubscriberClientSettingsBase = mqttSubscriberClientSettingsBase;
    }

    private MQTTSubscriberClientSettingsBase MqttSubscriberClientSettingsBase { get; }

    public Func<ArraySegment<byte>, string, Task>? ReceivedMessageAsync { get; set; }

    public async Task SubscribeAsync(CancellationToken cancellationToken)
    {
        if (!MqttClient.IsConnected)
        {
            var response = await MqttClient.ConnectAsync(CreateSettings(), cancellationToken);
            if (!response.ResultCode.Equals(MqttClientConnectResultCode.Success))
            {
                throw new SystemException();
            }

            MqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedFuncAsync;

            await MqttClient.SubscribeAsync(MqttSubscriberClientSettingsBase.Topic, MqttSubscriberClientSettingsBase.QualityOfServiceLevel, cancellationToken).ConfigureAwait(false);
        }

        while (!cancellationToken.IsCancellationRequested && MqttClient.IsConnected)
        {
            cancellationToken.WaitHandle.WaitOne(10000);
        }

        if (!cancellationToken.IsCancellationRequested && !MqttClient.IsConnected)
        {
            throw new Exception("Mqtt Client not connected");
        }
    }

    private MqttClientOptions CreateSettings()
    {
        MqttClientOptionsBuilderTlsParameters tlsOptions = new()
        {
            SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
            UseTls = true,
            AllowUntrustedCertificates = true
        };

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MqttSubscriberClientSettingsBase?.Connection?.Hostname ?? string.Empty, MqttSubscriberClientSettingsBase?.Connection?.Port)
            .WithCredentials(MqttSubscriberClientSettingsBase?.Connection?.Username ?? string.Empty, MqttSubscriberClientSettingsBase?.Connection?.Password ?? string.Empty)
            .WithClientId(Guid.NewGuid().ToString())
            .WithTls(tlsOptions)
            .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
            .Build();

        return options;
    }

    private async Task OnApplicationMessageReceivedFuncAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        try
        {
            if (!MqttClient.IsConnected)
            {
                throw new InvalidOperationException("Mqtt Client not connected.");
            }

            // TODO: fix possible null reference
            await ReceivedMessageAsync?.Invoke(args.ApplicationMessage.PayloadSegment, args.ApplicationMessage.Topic);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
