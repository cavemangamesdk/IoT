using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;
using MotionController.Sensor.Models.Game;
using MotionController.Services;

namespace MotionController.Sensor.Services;

public interface IGameSessionInputDataService : IService
{
    Task CreateGameSessionInputDataAsync(GameSession gameSession, Vector2 inputData);
    Task CreateGameSessionInputDataAsync(GameSession gameSession, IEnumerable<Vector2?> inputData);
}

internal class GameSessionInputDataService : ServiceBase<GameSessionInputDataService>, IGameSessionInputDataService
{
    public GameSessionInputDataService(ILogger<GameSessionInputDataService> logger, IServiceProvider serviceProvider)
        : base(logger)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task CreateGameSessionInputDataAsync(GameSession gameSession, Vector2 inputData)
    {
        if (inputData == default)
        {
            return;
        }

        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var gameSessionInputDataRepository = scope.ServiceProvider.GetRequiredService<IGameSessionInputDataRepository>();

        var gameSessionInputData = new GameSessionInputData
        {
            GameSessionId = gameSession.Id,
            X = inputData.X,
            Y = inputData.Y,
        };

        await gameSessionInputDataRepository.AddAsync(gameSessionInputData);

        unitOfWork.Complete();
    }

    public async Task CreateGameSessionInputDataAsync(GameSession gameSession, IEnumerable<Vector2?> inputData)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        foreach (var input in inputData)
        {
            if (input == default)
            {
                continue;
            }

            await CreateGameSessionInputDataAsync(gameSession, input);
        }

        unitOfWork.Complete();
    }
}
