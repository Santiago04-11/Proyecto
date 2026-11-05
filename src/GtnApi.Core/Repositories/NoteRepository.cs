using GtnApi.Core.Data;
using GtnApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly IDbContextFactory<GtnDbContext> _factory;

    public NoteRepository(IDbContextFactory<GtnDbContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<Note>> GetAllAsync(int? projectId = null, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var query = ctx.Notes.AsNoTracking();
        if (projectId.HasValue)
        {
            query = query.Where(n => n.ProjectId == projectId.Value);
        }

        return await query
            .OrderByDescending(n => n.UpdatedAt)
            .ThenByDescending(n => n.Id)
            .ToListAsync(ct);
    }

    public async Task<Note?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        return await ctx.Notes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<Note> AddAsync(Note note, CancellationToken ct = default)
    {
        var title = Guard.Required(note.Title, "El título", 200);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        await Guard.EnsureProjectExistsAsync(ctx, note.ProjectId, ct);

        var now = DateTime.UtcNow;
        var entity = new Note
        {
            Title = title,
            Content = note.Content ?? string.Empty,
            ProjectId = note.ProjectId,
            CreatedAt = now,
            UpdatedAt = now
        };
        ctx.Notes.Add(entity);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Note note, CancellationToken ct = default)
    {
        var title = Guard.Required(note.Title, "El título", 200);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var existing = await ctx.Notes.FirstOrDefaultAsync(n => n.Id == note.Id, ct);
        if (existing is null)
        {
            return false;
        }

        await Guard.EnsureProjectExistsAsync(ctx, note.ProjectId, ct);

        existing.Title = title;
        existing.Content = note.Content ?? string.Empty;
        existing.ProjectId = note.ProjectId;
        existing.UpdatedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var deleted = await ctx.Notes.Where(n => n.Id == id).ExecuteDeleteAsync(ct);
        return deleted > 0;
    }
}
