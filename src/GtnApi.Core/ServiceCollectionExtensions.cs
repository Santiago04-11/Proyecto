using GtnApi.Core.Data;
using GtnApi.Core.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GtnApi.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>Registra el DbContext (SQLite) y los repositorios del núcleo.</summary>
    public static IServiceCollection AddGtnCore(this IServiceCollection services, string? databasePath = null)
    {
        var path = Path.GetFullPath(databasePath ?? DatabasePaths.DefaultDatabasePath);
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var connectionString = new SqliteConnectionStringBuilder { DataSource = path }.ToString();
        services.AddDbContextFactory<GtnDbContext>(o => o.UseSqlite(connectionString));

        services.AddSingleton<IProjectRepository, ProjectRepository>();
        services.AddSingleton<INoteRepository, NoteRepository>();
        services.AddSingleton<ITaskRepository, TaskRepository>();
        return services;
    }
}
