using GtnApi.Core.Data;
using GtnApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IDbContextFactory<GtnDbContext> _factory;

    public ProjectRepository(IDbContextFactory<GtnDbContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        return await ctx.Projects.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        return await ctx.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken ct = default)
    {
        var name = Guard.Required(project.Name, "El nombre del proyecto", 100);
        var description = Guard.Optional(project.Description, "La descripción", 500);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var entity = new Project
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
        ctx.Projects.Add(entity);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Project project, CancellationToken ct = default)
    {
        var name = Guard.Required(project.Name, "El nombre del proyecto", 100);
        var description = Guard.Optional(project.Description, "La descripción", 500);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var existing = await ctx.Projects.FirstOrDefaultAsync(p => p.Id == project.Id, ct);
        if (existing is null)
        {
            return false;
        }

        existing.Name = name;
        existing.Description = description;
        await ctx.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);

        // Transacción: o se desasocian y se borra todo, o no se toca nada (RNF-03).
        await using var tx = await ctx.Database.BeginTransactionAsync(ct);

        var exists = await ctx.Projects.AnyAsync(p => p.Id == id, ct);
        if (!exists)
        {
            return false;
        }

        await ctx.Notes.Where(n => n.ProjectId == id)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.ProjectId, (int?)null), ct);
        await ctx.Tasks.Where(t => t.ProjectId == id)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.ProjectId, (int?)null), ct);
        await ctx.Projects.Where(p => p.Id == id).ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);
        return true;
    }
}
