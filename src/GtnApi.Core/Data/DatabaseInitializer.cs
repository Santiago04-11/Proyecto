using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Data;

public static class DatabaseInitializer
{
    /// <summary>Crea la base de datos si no existe y aplica las migraciones pendientes.</summary>
    public static async Task MigrateAsync(
        IDbContextFactory<GtnDbContext> factory,
        CancellationToken ct = default)
    {
        await using var ctx = await factory.CreateDbContextAsync(ct);
        await ctx.Database.MigrateAsync(ct);
    }
}
