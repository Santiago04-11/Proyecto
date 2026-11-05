using GtnApi.Core.Data;
using GtnApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDbContextFactory<GtnDbContext> _factory;

    public TaskRepository(IDbContextFactory<GtnDbContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(
        int? projectId = null,
        TaskItemStatus? status = null,
        CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var query = ctx.Tasks.AsNoTracking();
        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .ThenByDescending(t => t.Id)
            .ToListAsync(ct);
    }

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        return await ctx.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default)
    {
        var title = Guard.Required(task.Title, "El título", 200);
        var description = Guard.Optional(task.Description, "La descripción", 2000);
        EnsureValidStatus(task.Status);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        await Guard.EnsureProjectExistsAsync(ctx, task.ProjectId, ct);

        var now = DateTime.UtcNow;
        var entity = new TaskItem
        {
            Title = title,
            Description = description,
            Status = task.Status,
            ProjectId = task.ProjectId,
            CreatedAt = now,
            UpdatedAt = now
        };
        ctx.Tasks.Add(entity);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(TaskItem task, CancellationToken ct = default)
    {
        var title = Guard.Required(task.Title, "El título", 200);
        var description = Guard.Optional(task.Description, "La descripción", 2000);
        EnsureValidStatus(task.Status);

        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var existing = await ctx.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id, ct);
        if (existing is null)
        {
            return false;
        }

        await Guard.EnsureProjectExistsAsync(ctx, task.ProjectId, ct);

        existing.Title = title;
        existing.Description = description;
        existing.Status = task.Status;
        existing.ProjectId = task.ProjectId;
        existing.UpdatedAt = DateTime.UtcNow;
        await ctx.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = await _factory.CreateDbContextAsync(ct);
        var deleted = await ctx.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync(ct);
        return deleted > 0;
    }

    private static void EnsureValidStatus(TaskItemStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentException("El estado de la tarea no es válido.");
        }
    }
}
