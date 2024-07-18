using System.Data;
using Npgsql;

namespace ThreadBasket.Data;

public class DapperContext(string connectionString)
{
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(connectionString);
}