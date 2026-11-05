using GtnApi.Core.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Tests;

/// <summary>
/// Base de datos SQLite en memoria para tests. Aplica las migraciones reales
/// y mantiene la conexión abierta mientras viva el objeto.
/// </summary>
public sealed class TestDb : IDbContextFactory<GtnDbContext>, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<GtnDbContext> _options;

    public TestDb()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
        }

        _options = new DbContextOptionsBuilder<GtnDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var ctx = CreateDbContext();
        ctx.Database.Migrate();
    }

    public GtnDbContext CreateDbContext() => new(_options);

    public void Dispose() => _connection.Dispose();
}
