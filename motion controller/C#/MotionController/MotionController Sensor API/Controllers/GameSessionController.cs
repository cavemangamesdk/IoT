using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Models.Game;
using MotionController.Sensor.Services;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

[Route("api/v1/game/sessions")]
[OpenApiController("GameSession")]
public class GameSessionController : ControllerBase
{
    public GameSessionController(ILogger<GameSessionController> logger, IGameSessionService gameSessionService)
    {
        Logger = logger;
        GameSessionService = gameSessionService;
    }

    private ILogger<GameSessionController> Logger { get; }
    private IGameSessionService GameSessionService { get; }

    [HttpGet]
    [Route("", Name = nameof(GetGameSessionsAsync))]
    [OpenApiOperation(nameof(GetGameSessionsAsync), "Gets all Game Sessions", "")]
    [ProducesResponseType(typeof(IEnumerable<GameSession?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameSessionsAsync()
    {
        try
        {
            var gameSessions = await GameSessionService.GetGameSessionsAsync();
            if (!gameSessions.Any())
            {
                return NotFound();
            }

            return Ok(gameSessions);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetGameSessionsAsync)} operation failed.");
            throw;
        }
    }

    [HttpGet]
    [Route("{sessionId:guid}", Name = nameof(GetGameSessionBySessionIdAsync))]
    [OpenApiOperation(nameof(GetGameSessionBySessionIdAsync), "Get a Game Session by Session Id", "")]
    [ProducesResponseType(typeof(GameSession), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameSessionBySessionIdAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var gameSession = await GameSessionService.GetGameSessionAsync(sessionId);
            if (gameSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            return Ok(gameSession);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetGameSessionBySessionIdAsync)} operation failed.");
            throw;
        }
    }

    [HttpPost]
    [Route("", Name = nameof(AddGameSessionAsync))]
    [OpenApiOperation(nameof(AddGameSessionAsync), "Adds a Game Session", "")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGameSessionAsync([FromBody] UnityGameSession unityGameSession)
    {
        try
        {
            var created = await GameSessionService.CreateGameSessionAsync(unityGameSession);
            if (created)
            {
                //var gameSession = await GameSessionService.GetGameSessionAsync(unityGameSession.Guid);
                //return CreatedAtRoute(nameof(GetGameSessionBySessionIdAsync), new { sessionId = unityGameSession.Guid }, gameSession);

                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(AddGameSessionAsync)} operation failed.");
            throw;
        }
    }
}
