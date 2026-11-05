using GtnApi.Api.Dtos;
using GtnApi.Core.Models;
using GtnApi.Core.Repositories;

namespace GtnApi.Api.Endpoints;

public static class NotesEndpoints
{
    public static void MapNotesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notes").WithTags("Notes");

        group.MapGet("/", async (INoteRepository repo, int? projectId, CancellationToken ct) =>
        {
            var notes = await repo.GetAllAsync(projectId, ct);
            return Results.Ok(notes.Select(n => new NoteDto(n.Id, n.Title, n.Content, n.ProjectId, n.CreatedAt, n.UpdatedAt)));
        });

        group.MapGet("/{id:int}", async (int id, INoteRepository repo, CancellationToken ct) =>
        {
            var note = await repo.GetByIdAsync(id, ct);
            return note is null ? Results.NotFound() : Results.Ok(new NoteDto(note.Id, note.Title, note.Content, note.ProjectId, note.CreatedAt, note.UpdatedAt));
        });

        group.MapPost("/", async (CreateNoteRequest req, INoteRepository repo, CancellationToken ct) =>
        {
            try
            {
                var note = await repo.AddAsync(new Note { Title = req.Title, Content = req.Content ?? "", ProjectId = req.ProjectId }, ct);
                return Results.Created($"/api/notes/{note.Id}", new NoteDto(note.Id, note.Title, note.Content, note.ProjectId, note.CreatedAt, note.UpdatedAt));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateNoteRequest req, INoteRepository repo, CancellationToken ct) =>
        {
            try
            {
                var ok = await repo.UpdateAsync(new Note { Id = id, Title = req.Title, Content = req.Content ?? "", ProjectId = req.ProjectId }, ct);
                return ok ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapDelete("/{id:int}", async (int id, INoteRepository repo, CancellationToken ct) =>
        {
            var ok = await repo.DeleteAsync(id, ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
