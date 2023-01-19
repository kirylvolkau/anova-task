using System.Data;
using Npgsql;

namespace AnovaTask.API.Storage;

/// <summary>
/// Global context for the database operations.
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    /// <summary>
    /// Name of the table for storing devices.
    /// </summary>
    public readonly static string DevicesTable = "devices";

    /// <summary>
    /// Name of the table for storing readings.
    /// </summary>
    public readonly static string ReadingsTable = "readings";

    /// <summary>
    /// Constructor for DapperContext.
    /// </summary>
    /// <param name="configuration">Configuration to get connection string.</param>
    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres") ?? throw new Exception("No connection string.");
    }

    /// <summary>
    /// Creates database connection.
    /// </summary>
    /// <returns>Database connection.</returns>
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}