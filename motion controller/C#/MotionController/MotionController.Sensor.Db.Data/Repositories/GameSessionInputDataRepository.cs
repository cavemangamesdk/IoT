using MotionController.Data.Repositories;
using MotionController.Data.Repositories.Database;
using MotionController.Sensor.Db.Data.Models;
using MotionController.Sensor.Db.Data.Providers;

namespace MotionController.Sensor.Db.Data.Repositories;

public interface IGameSessionInputDataRepository : IRepository<GameSessionInputData, int>
{
}

internal class GameSessionInputDataRepository : DbRepositoryBase<GameSessionInputData, int>, IGameSessionInputDataRepository
{
    public GameSessionInputDataRepository(IMotionProvider provider)
        : base(provider)
    {
    }

    public override Task<bool> AddAsync(GameSessionInputData model)
    {
        model.Created = DateTime.Now;
        model.Modified = DateTime.Now;

        return base.AddAsync(model);
    }
}