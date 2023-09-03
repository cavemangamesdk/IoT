using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IGameSessionRepository : IRepository<GameSession, int>
{
    Task<GameSession?> GetAsync(Guid sessionId);
}

internal class GameSessionRepository : DbRepositoryBase<GameSession, int>, IGameSessionRepository
{
    public GameSessionRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public async Task<GameSession?> GetAsync(Guid sessionId)
    {
        var query = $@"SELECT * FROM {Provider.GetQualifiedTableName<GameSession>()} WHERE ( [SessionId] = @sessionId )";

        return await QuerySingleOrDefaultAsync(query, new { @sessionId });
    }

    public override Task<bool> AddAsync(GameSession model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}
