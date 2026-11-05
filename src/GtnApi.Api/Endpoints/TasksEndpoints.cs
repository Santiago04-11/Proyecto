using GtnApi.Api.Dtos;
using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Api.Endpoints;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        group.MapGet("/", async (ITaskRepository repo, int? projectId, TaskItemStatus? status, CancellationToken ct) =>
        {
            var tasks = await repo.GetAllAsync(projectId, status, ct);
            return Results.Ok(tasks.Select(t => new TaskDto(t.Id, t.Title, t.Description, t.Status.ToString(), t.ProjectId, t.CreatedAt, t.UpdatedAt)));
        });

        group.MapGet("/{id:int}", async (int id, ITaskRepository repo, CancellationToken ct) =>
        {
            var task = await repo.GetByIdAsync(id, ct);
            return task is null ? Results.NotFound() : Results.Ok(new TaskDto(task.Id, task.Title, task.Description, task.Status.ToString(), task.ProjectId, task.CreatedAt, task.UpdatedAt));
        });

        group.MapPost("/", async (CreateTaskRequest req, ITaskRepository repo, CancellationToken ct) =>
        {
            try
            {
                var task = await repo.AddAsync(new TaskItem { Title = req.Title, Description = req.Description, Status = req.Status, ProjectId = req.ProjectId }, ct);
                return Results.Created($"/api/tasks/{task.Id}", new TaskDto(task.Id, task.Title, task.Description, task.Status.ToString(), task.ProjectId, task.CreatedAt, task.UpdatedAt));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateTaskRequest req, ITaskRepository repo, CancellationToken ct) =>
        {
            try
            {
                var ok = await repo.UpdateAsync(new TaskItem { Id = id, Title = req.Title, Description = req.Description, Status = req.Status, ProjectId = req.ProjectId }, ct);
                return ok ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapDelete("/{id:int}", async (int id, ITaskRepository repo, CancellationToken ct) =>
        {
            var ok = await repo.DeleteAsync(id, ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
