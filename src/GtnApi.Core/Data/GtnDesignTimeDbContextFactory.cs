using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GtnApi.Core.Data;

/// <summary>
/// Solo se usa en tiempo de diseño (comandos "dotnet ef"). No se ejecuta en la app final.
/// </summary>
public class GtnDesignTimeDbContextFactory : IDesignTimeDbContextFactory<GtnDbContext>
{
    public GtnDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<GtnDbContext>()
            .UseSqlite("Data Source=gtn-design.db")
            .Options;
        return new GtnDbContext(options);
    }
}
