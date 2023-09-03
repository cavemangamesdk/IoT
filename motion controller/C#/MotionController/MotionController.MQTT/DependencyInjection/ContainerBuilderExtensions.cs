using Autofac;
using Microsoft.Extensions.Options;
using MotionController.MQTT;
using MotionController.MQTT.Client.Subscriber;
using MotionController.MQTT.Messages;
using MotionController.MQTT.Settings;
using MQTTnet;
using MQTTnet.Client;
using System.Reflection;

namespace MotionController.Extensions.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterMQTT(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<MqttFactory>()
            .AsSelf()
            .SingleInstance();

        containerBuilder.Register((cc, p) =>
        {
            var mqttFactory = cc.Resolve<MqttFactory>();

            return mqttFactory.CreateMqttClient();
        })
            .As<IMqttClient>()
            .SingleInstance();

        return containerBuilder;
    }

    public static ContainerBuilder WithConnection<TMQTTSettings>(this ContainerBuilder containerBuilder, string clientId)
        where TMQTTSettings : MQTTSettingsBase
    {
        containerBuilder.Register((cc) =>
        {
            var mqttSettings = cc.Resolve<IOptions<TMQTTSettings>>()?.Value ?? default;
            if (mqttSettings == default)
            {
                throw new Exception();
            }

            MqttClientOptionsBuilderTlsParameters tlsOptions = new()
            {
                SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                UseTls = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttSettings?.Connection?.Hostname ?? string.Empty, mqttSettings?.Connection?.Port)
                .WithCredentials(mqttSettings?.Connection?.Username ?? string.Empty, mqttSettings?.Connection?.Password ?? string.Empty)
                .WithClientId(clientId)
                .WithTls(tlsOptions)
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .Build();

            return options;
        })
            .As<MqttClientOptions>()
            .SingleInstance();

        containerBuilder.Register((cc) =>
        {
            var serviceProvider = cc.Resolve<IServiceProvider>();

            return new MQTTSubscriberClientFactory(serviceProvider);
        })
            .As<IMQTTSubscriberClientFactory>()
            .InstancePerLifetimeScope();

        return containerBuilder;
    }

    public static ContainerBuilder WithMessageHandlers(this ContainerBuilder containerBuilder, Assembly assembly)
    {
        var messageHandlers = assembly.GetTypes().Where(t => t.IsAssignableTo<IMessageHandler>() && t.GetCustomAttributes<MQTTTopicAttribute>().Any());
        foreach (var messageHandler in messageHandlers)
        {
            var attributes = messageHandler.GetCustomAttributes<MQTTTopicAttribute>();
            foreach (var attribute in attributes)
            {
                containerBuilder.RegisterType(messageHandler).Keyed<IMessageHandler>(attribute.Topic);
            }
        }

        return containerBuilder;
    }
}
