using Autofac;
using Autofac.Extensions.DependencyInjection;
using MotionController.MQTT.Messages;

namespace MotionController.Sensor.Messaging;

public interface IMessageHandlerResolver
{
    IMessageHandler? Resolve(string topic);
    TMessageHandler? Resolve<TMessageHandler>(string topic) where TMessageHandler : IMessageHandler;
}

internal class MessageHandlerResolver : IMessageHandlerResolver
{
    public MessageHandlerResolver(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public IMessageHandler? Resolve(string topic)
    {
        return ServiceProvider.GetAutofacRoot().ResolveKeyed<IMessageHandler>(topic);
    }

    public TMessageHandler? Resolve<TMessageHandler>(string topic) where TMessageHandler : IMessageHandler
    {
        return ServiceProvider.GetAutofacRoot().ResolveKeyed<TMessageHandler>(topic);
    }
}
