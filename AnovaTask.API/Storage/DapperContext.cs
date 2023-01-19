using System.Data;
using Npgsql;

namespace AnovaTask.API.Storage;

public class DapperContext
{
    private readonly string _connectionString;

    public readonly static string DevicesTable = "devices";

    public readonly static string ReadingsTable = "readings";

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres") ?? throw new Exception("No connection string.");
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}