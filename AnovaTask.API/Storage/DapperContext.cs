using System.Data;
using Npgsql;

namespace AnovaTask.API.Storage;

public class DapperContext
{
    private readonly string _connectionString;
    
    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres");
    }
    
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}