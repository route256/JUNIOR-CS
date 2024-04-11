using System.Transactions;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Npgsql;

namespace HomeworkApp.Dal.Repositories;

public abstract class PgRepository : IPgRepository
{
    private readonly DalOptions _dalSettings;

    protected const int DefaultTimeoutInSeconds = 5;

    protected PgRepository(DalOptions dalSettings)
    {
        _dalSettings = dalSettings;
    }
    
    protected async Task<NpgsqlConnection> GetConnection()
    {
        if (Transaction.Current is not null &&
            Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted)
        {
            throw new TransactionAbortedException("Transaction was aborted (probably by user cancellation request)");
        }
        
        var connection = new NpgsqlConnection(_dalSettings.PostgresConnectionString);
        await connection.OpenAsync();
        
        // Due to in-process migrations
        connection.ReloadTypes();
        
        return connection;
    }
}