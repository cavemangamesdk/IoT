using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MotionController.MQTT.Messages
{
    public interface IMessageHandler
    {
        Task HandleAsync(string jsonMessage);
        Task HandleAsync(object model);
    }

    public interface IMessageHandler<TModel> : IMessageHandler
    {
    }

    public abstract class MessageHandlerBase<TMessageHandler, TModel> : IMessageHandler<TModel>
        where TMessageHandler : IMessageHandler
    {
        public MessageHandlerBase(ILogger<TMessageHandler> logger, IServiceProvider serviceProvider)
        {
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        protected ILogger<TMessageHandler> Logger { get; }
        protected IServiceProvider ServiceProvider { get; }

        public Task HandleAsync(object model)
        {
            if (model is TModel castedModel)
            {
                using var scope = ServiceProvider.CreateScope();

                return HandleModelAsync(castedModel);
            }

            throw new InvalidCastException("Model is invald!");
        }

        public Task HandleAsync(string jsonMessage)
        {
            var model = JsonConvert.DeserializeObject<TModel>(jsonMessage);
            if (model?.Equals(default) ?? true)
            {
                Logger.LogError($"Failed to deserialize JSON string:\n{jsonMessage}");
                return Task.CompletedTask;
            }

            return HandleAsync(model);
        }

        protected abstract Task HandleModelAsync(TModel model);
    }
}
