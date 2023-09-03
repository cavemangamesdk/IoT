using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;
using MotionController.Sensor.Models.Game;
using MotionController.Services;

namespace MotionController.Sensor.Services;

public interface IGameSessionBoardRotationService : IService
{
    Task CreateGameSessionBoardRotationAsync(GameSession gameSession, Vector3 boardRotation);
    Task CreateGameSessionBoardRotationsAsync(GameSession gameSession, IEnumerable<Vector3?> boardRotations);
}

internal class GameSessionBoardRotationService : ServiceBase<GameSessionBoardRotationService>, IGameSessionBoardRotationService
{
    public GameSessionBoardRotationService(ILogger<GameSessionBoardRotationService> logger, IServiceProvider serviceProvider) 
        : base(logger)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task CreateGameSessionBoardRotationAsync(GameSession gameSession, Vector3 boardRotation)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var gameSessionBoardRotationRepository = scope.ServiceProvider.GetRequiredService<IGameSessionBoardRotationRepository>();

        var gameSessionBoardRotation = new GameSessionBoardRotation
        {
            GameSessionId = gameSession.Id,
            X = boardRotation.X,
            Y = boardRotation.Y,
            Z = boardRotation.Z
        };

        await gameSessionBoardRotationRepository.AddAsync(gameSessionBoardRotation);

        unitOfWork.Complete();
    }

    public async Task CreateGameSessionBoardRotationsAsync(GameSession gameSession, IEnumerable<Vector3?> boardRotations)
    {
        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        foreach (var boardRotation in boardRotations)
        {
            if (boardRotation == default)
            {
                continue;
            }

            await CreateGameSessionBoardRotationAsync(gameSession, boardRotation);
        }

        unitOfWork.Complete();
    }
}
