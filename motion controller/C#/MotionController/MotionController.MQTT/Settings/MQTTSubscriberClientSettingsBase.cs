namespace MotionController.MQTT.Settings;

public class MQTTSubscriberClientSettingsBase : MQTTSettingsBase
{
    public MQTTnet.Protocol.MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }
    public string? Topic { get; set; }
}