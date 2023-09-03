using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IGameSessionBallPositionRepository : IRepository<GameSessionBallPosition, int>
{
}

internal class GameSessionBallPositionRepository : DbRepositoryBase<GameSessionBallPosition, int>, IGameSessionBallPositionRepository
{
    public GameSessionBallPositionRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public override Task<bool> AddAsync(GameSessionBallPosition model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}
