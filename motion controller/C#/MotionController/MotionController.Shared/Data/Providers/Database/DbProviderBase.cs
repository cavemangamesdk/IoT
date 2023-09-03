using Dapper;
using Dapper.Contrib.Extensions;
using MotionController.Data.Models;
using System.Data;

namespace MotionController.Data.Providers.Database;

public interface IDbProvider : IProvider
{
    string GetQualifiedTableName<TModel>() where TModel : DatabaseModel;

    Task<TModel?> GetAsync<TModel, TKey>(TKey key, int? commandTimeout = null)
        where TModel : DatabaseModel
        where TKey : notnull;

    Task<IEnumerable<TModel>> GetAllAsync<TModel>(int? commandTimeout = null)
        where TModel : DatabaseModel;

    Task<bool> InsertAsync<TModel>(TModel model, int? commandTimeout = null)
        where TModel : DatabaseModel;

    Task<bool> UpdateAsync<TModel>(TModel model, int? commandTimeout = null)
        where TModel : DatabaseModel;

    Task<bool> DeleteAsync<TModel>(TModel model, int? commandTimeout = null)
        where TModel : DatabaseModel;

    Task<bool> DeleteAllAsync<TModel>(int? commandTimeout = null)
        where TModel : DatabaseModel;

    Task<TModel> QuerySingleAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
        where TModel : DatabaseModel;

    Task<TModel> QuerySingleOrDefaultAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
        where TModel : DatabaseModel;

    Task<TModel> QueryFirstAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
        where TModel : DatabaseModel;

    Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
        where TModel : DatabaseModel;

    Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
        where TModel : DatabaseModel;

    Task<int> ExecuteAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null);
    Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null);
    Task<T> ExecuteSingleOrDefaultAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null);
    Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null);
}

public abstract class DbProviderBase : ProviderBase, IDbProvider
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _committed;

    public DbProviderBase(IsolationLevel isolationLevel)
        : base(isolationLevel)
    {
    }

    public IDbConnection Connection => _connection ??= CreateConnection();

    public IDbTransaction Transaction
    {
        get
        {
            if (_transaction != null)
            {
                return _transaction;
            }

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            return _transaction = Connection.BeginTransaction();
        }
    }

    protected abstract IDbConnection CreateConnection();

    public string GetQualifiedTableName<TModel>()
        where TModel : DatabaseModel
    {
        return Connection.GetQualifiedTableName<TModel>();
    }

    public async virtual Task<TModel?> GetAsync<TModel, TKey>(TKey key, int? commandTimeout = null)
        where TModel : DatabaseModel
        where TKey : notnull
    {
        return await Connection.GetAsync<TModel>(new object[] { key }, Transaction, commandTimeout);
    }

    public async virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(int? commandTimeout = null) where TModel : DatabaseModel
    {
        return await Connection.GetAllAsync<TModel>(Transaction, commandTimeout);
    }

    public async virtual Task<bool> InsertAsync<TModel>(TModel model, int? commandTimeout = null) where TModel : DatabaseModel
    {
        return await Connection.InsertAsync<TModel>(model, Transaction, commandTimeout);
    }

    public async virtual Task<bool> UpdateAsync<TModel>(TModel model, int? commandTimeout = null) where TModel : DatabaseModel
    {
        return await Connection.UpdateAsync<TModel>(model, Transaction, commandTimeout);
    }

    public async virtual Task<bool> DeleteAsync<TModel>(TModel model, int? commandTimeout = null) where TModel : DatabaseModel
    {
        return await Connection.DeleteAsync<TModel>(model, Transaction, commandTimeout);
    }

    public async virtual Task<bool> DeleteAllAsync<TModel>(int? commandTimeout = null) where TModel : DatabaseModel
    {
        return await Connection.DeleteAllAsync<TModel>(Transaction, commandTimeout);
    }

    public async virtual Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null) where TModel : DatabaseModel
    {
        return await Connection.QueryAsync<TModel>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public async virtual Task<TModel> QueryFirstAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null) where TModel : DatabaseModel
    {
        return await Connection.QueryFirstAsync<TModel>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public async virtual Task<TModel> QueryFirstOrDefaultAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null) where TModel : DatabaseModel
    {
        return await Connection.QueryFirstOrDefaultAsync<TModel>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public async virtual Task<TModel> QuerySingleAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null) where TModel : DatabaseModel
    {
        return await Connection.QuerySingleAsync<TModel>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public async virtual Task<TModel> QuerySingleOrDefaultAsync<TModel>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null) where TModel : DatabaseModel
    {
        return await Connection.QuerySingleOrDefaultAsync<TModel>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public async Task<int> ExecuteAsync(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return await Connection.ExecuteAsync(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return Connection.ExecuteScalarAsync<T>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public Task<T> ExecuteSingleOrDefaultAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return Connection.QuerySingleOrDefaultAsync<T>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object? parameters = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return Connection.QueryAsync<T>(sql, parameters, Transaction, commandTimeout, commandType);
    }

    public override void Commit()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Commit();
        _committed = true;
    }

    public override void Rollback()
    {
        if (_transaction == null)
        {
            return;
        }

        _transaction.Rollback();
    }

    protected override void DisposeManagedState()
    {
        if (_transaction != null)
        {
            if (!_committed)
            {
                Rollback();
            }

            _transaction.Dispose();
        }

        if (_connection != null)
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }

            _connection.Dispose();
        }
    }
}