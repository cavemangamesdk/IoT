namespace MotionController.Data.Repositories;

public interface IRepository
{
}

public interface IRepository<TModel> : IRepository
    where TModel : class
{
}

public interface IWriteRepository<TModel> : IRepository<TModel>
    where TModel : class
{
    Task<bool> AddAsync(TModel model);
    Task<bool> UpdateAsync(TModel model);
    Task<bool> AddOrUpdateAsync(TModel model);
    Task<bool> DeleteAsync(TModel model);
    Task<bool> DeleteAllAsync();
}

public interface IReadOnlyRepository<TModel> : IRepository<TModel>
    where TModel : class
{
    Task<TModel?> GetFirstOrDefault();
    Task<IEnumerable<TModel?>> GetAsync();
}

public interface IReadOnlyRepository<TModel, TKey1> : IReadOnlyRepository<TModel>
    where TModel : class
    where TKey1 : notnull
{
    Task<TModel?> GetAsync(TKey1 key);
}

public interface IRepository<TModel, TKey1> : IWriteRepository<TModel>, IReadOnlyRepository<TModel, TKey1>
    where TModel : class
    where TKey1 : notnull
{
}
