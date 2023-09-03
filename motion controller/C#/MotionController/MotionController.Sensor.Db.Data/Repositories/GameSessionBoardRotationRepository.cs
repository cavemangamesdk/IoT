using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IGameSessionBoardRotationRepository : IRepository<GameSessionBoardRotation, int>
{
}

internal class GameSessionBoardRotationRepository : DbRepositoryBase<GameSessionBoardRotation, int>, IGameSessionBoardRotationRepository
{
    public GameSessionBoardRotationRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public override Task<bool> AddAsync(GameSessionBoardRotation model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}
