using GtnApi.Api.Dtos;
using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Api.Endpoints;

public static class ProjectsEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        group.MapGet("/", async (IProjectRepository repo, CancellationToken ct) =>
        {
            var projects = await repo.GetAllAsync(ct);
            return Results.Ok(projects.Select(p => new ProjectDto(p.Id, p.Name, p.Description, p.CreatedAt)));
        });

        group.MapGet("/{id:int}", async (int id, IProjectRepository repo, CancellationToken ct) =>
        {
            var project = await repo.GetByIdAsync(id, ct);
            return project is null ? Results.NotFound() : Results.Ok(new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt));
        });

        group.MapPost("/", async (CreateProjectRequest req, IProjectRepository repo, CancellationToken ct) =>
        {
            try
            {
                var project = await repo.AddAsync(new Project { Name = req.Name, Description = req.Description }, ct);
                return Results.Created($"/api/projects/{project.Id}", new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateProjectRequest req, IProjectRepository repo, CancellationToken ct) =>
        {
            try
            {
                var ok = await repo.UpdateAsync(new Project { Id = id, Name = req.Name, Description = req.Description }, ct);
                return ok ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapDelete("/{id:int}", async (int id, IProjectRepository repo, CancellationToken ct) =>
        {
            var ok = await repo.DeleteAsync(id, ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
