using MotionController.Data.Models;
using MotionController.Data.Providers.Database;
using System.Data;

namespace MotionController.Data.Repositories.Database;

public abstract class DbRepositoryBase : RepositoryBase<IDbProvider>
{
    protected DbRepositoryBase(IDbProvider provider)
        : base(provider)
    {
    }

    protected async Task<int> ExecuteAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.ExecuteAsync(sql, parameters, commandTimeout, commandType);
    }

    protected async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.ExecuteScalarAsync<T>(sql, parameters, commandTimeout, commandType);
    }

    protected async Task<T> ExecuteSingleOrDefaultAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.ExecuteSingleOrDefaultAsync<T>(sql, parameters, commandTimeout, commandType);
    }

    protected async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.ExecuteQueryAsync<T>(sql, parameters, commandTimeout, commandType);
    }
}

public abstract class DbReadOnlyRepositoryBase<TModel> : DbRepositoryBase, IReadOnlyRepository<TModel>
    where TModel : DatabaseModel
{
    protected DbReadOnlyRepositoryBase(IDbProvider provider)
        : base(provider)
    {
    }

    protected async virtual Task<TModel?> QuerySingleAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.QuerySingleAsync<TModel>(sql, parameters, commandTimeout, commandType);
    }

    protected async virtual Task<TModel?> QuerySingleOrDefaultAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.QuerySingleOrDefaultAsync<TModel>(sql, parameters, commandTimeout, commandType);
    }

    protected async virtual Task<TModel?> QueryFirstAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.QueryFirstAsync<TModel>(sql, parameters, commandTimeout, commandType);
    }

    protected async virtual Task<TModel?> QueryFirstOrDefaultAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.QueryFirstOrDefaultAsync<TModel>(sql, parameters, commandTimeout, commandType);
    }

    protected async virtual Task<IEnumerable<TModel?>> QueryAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Provider.QueryAsync<TModel>(sql, parameters, commandTimeout, commandType);
    }

    public abstract Task<IEnumerable<TModel?>> GetAsync();

    public async virtual Task<TModel?> GetFirstOrDefault()
    {
        var result = await GetAsync();
        if (result == default)
        {
            return default;
        }

        return result.FirstOrDefault();
    }
}

public abstract class DbReadOnlyRepositoryBase<TModel, TKey> : DbReadOnlyRepositoryBase<TModel>, IReadOnlyRepository<TModel, TKey>
    where TModel : DatabaseModel
    where TKey : notnull
{
    protected DbReadOnlyRepositoryBase(IDbProvider provider)
        : base(provider)
    {
    }

    public async Task<TModel?> GetAsync(TKey key)
    {
        return await Provider.GetAsync<TModel, TKey>(key);
    }

    public override async Task<IEnumerable<TModel?>> GetAsync()
    {
        return await Provider.GetAllAsync<TModel>();
    }
}

public abstract class DbRepositoryBase<TModel, TKey> : DbReadOnlyRepositoryBase<TModel, TKey>, IRepository<TModel, TKey>
    where TModel : DatabaseModel
    where TKey : notnull
{
    protected DbRepositoryBase(IDbProvider provider)
        : base(provider)
    {
    }

    public async virtual Task<bool> AddAsync(TModel model)
    {
        return await Provider.InsertAsync(model);
    }

    public async virtual Task<bool> UpdateAsync(TModel model)
    {
        return await Provider.UpdateAsync(model);
    }

    public async virtual Task<bool> AddOrUpdateAsync(TModel model)
    {
        if (model.IsTransient())
        {
            return await AddAsync(model);
        }

        return await UpdateAsync(model);
    }

    public async virtual Task<bool> DeleteAsync(TModel model)
    {
        return await Provider.DeleteAsync(model);
    }

    public async virtual Task<bool> DeleteAllAsync()
    {
        return await Provider.DeleteAllAsync<TModel>();
    }
}