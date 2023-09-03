using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionController.Data;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Repositories;
using MotionController.Sensor.Models.Game;
using MotionController.Services;
using System.Globalization;

namespace MotionController.Sensor.Services;

public interface IGameSessionService : IService
{
    Task<IEnumerable<GameSession?>> GetGameSessionsAsync();
    Task<GameSession?> GetGameSessionAsync(Guid sessionId);
    Task<bool> CreateGameSessionAsync(UnityGameSession gameSession);
}

internal class GameSessionService : ServiceBase<GameSessionService>, IGameSessionService
{
    private const string GameTimeFormat = @"m\:s\:ff";

    public GameSessionService(ILogger<GameSessionService> logger, IServiceProvider serviceProvider, IGameSessionRepository gameSessionRepository, IGameSessionBallPositionService gameSessionBallPositionService, IGameSessionBoardRotationService gameSessionBoardRotationService, IGameSessionInputDataService gameSessionInputDataService)
        : base(logger)
    {
        ServiceProvider = serviceProvider;
        GameSessionRepository = gameSessionRepository;
        GameSessionBallPositionService = gameSessionBallPositionService;
        GameSessionBoardRotationService = gameSessionBoardRotationService;
        GameSessionInputDataService = gameSessionInputDataService;
    }

    private IServiceProvider ServiceProvider { get; }
    private IGameSessionRepository GameSessionRepository { get; }
    private IGameSessionBallPositionService GameSessionBallPositionService { get; }
    private IGameSessionBoardRotationService GameSessionBoardRotationService { get; }
    private IGameSessionInputDataService GameSessionInputDataService { get; }

    public async Task<IEnumerable<GameSession?>> GetGameSessionsAsync()
    {
        return await GameSessionRepository.GetAsync();
    }

    public async Task<GameSession?> GetGameSessionAsync(Guid sessionId)
    {
        return await GameSessionRepository.GetAsync(sessionId);
    }

    public async Task<bool> CreateGameSessionAsync(UnityGameSession unityGameSession)
    {
        if (unityGameSession == default)
        {
            return false;
        }

        var gameSession = await CreateGameSessionAsyncCore(unityGameSession);
        if (gameSession?.Equals(default) ?? true)
        {
            return false;
        }

        var ballPositions = unityGameSession.GameData.Select(x => x.BallPosition);

        await GameSessionBallPositionService.CreateGameSessionBallPositionsAsync(gameSession, ballPositions);

        var boardRotations = unityGameSession.GameData.Select(x => x.BoardRotation);

        await GameSessionBoardRotationService.CreateGameSessionBoardRotationsAsync(gameSession, boardRotations);

        var inputData = unityGameSession.GameData.Select(x => x.InputData);

        await GameSessionInputDataService.CreateGameSessionInputDataAsync(gameSession, inputData);

        return true;
    }

    private async Task<GameSession?> CreateGameSessionAsyncCore(UnityGameSession unityGameSession)
    {
        if (unityGameSession?.Equals(default) ?? true)
        {
            return default;
        }

        if (!TimeSpan.TryParseExact(unityGameSession?.PlayerData?.GameTime ?? string.Empty, GameTimeFormat, CultureInfo.InvariantCulture, out var gameTimeSpan))
        {
            return default;
        }

        using var scope = ServiceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var gameSessionRepository = scope.ServiceProvider.GetRequiredService<IGameSessionRepository>();

        var gameSession = new GameSession
        {
            SessionId = unityGameSession.Guid,
            PlayerName = unityGameSession.PlayerData?.Name ?? string.Empty,
            Lives = unityGameSession.PlayerData?.Lives ?? default,
            GameTime = gameTimeSpan
        };

        var created = await gameSessionRepository.AddAsync(gameSession);
        if (!created)
        {
            return default;
        }

        unitOfWork.Complete();

        return gameSession;
    }
}
