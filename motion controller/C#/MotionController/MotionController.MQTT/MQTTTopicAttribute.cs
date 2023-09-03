namespace MotionController.MQTT;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class MQTTTopicAttribute : Attribute
{
    public MQTTTopicAttribute(string topic)
    {
        Topic = topic;
    }

    public string Topic { get; }
}
