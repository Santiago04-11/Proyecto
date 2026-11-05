using GtnApi.Core.Models;

namespace GtnApi.Api.Dtos;

// --- PROYECTOS ---
public record ProjectDto(int Id, string Name, string? Description, DateTime CreatedAt);
public record CreateProjectRequest(string Name, string? Description);
public record UpdateProjectRequest(string Name, string? Description);

// --- NOTAS ---
public record NoteDto(int Id, string Title, string Content, int? ProjectId, DateTime CreatedAt, DateTime UpdatedAt);
public record CreateNoteRequest(string Title, string? Content, int? ProjectId);
public record UpdateNoteRequest(string Title, string? Content, int? ProjectId);

// --- TAREAS ---
public record TaskDto(int Id, string Title, string? Description, string Status, int? ProjectId, DateTime CreatedAt, DateTime UpdatedAt);
public record CreateTaskRequest(string Title, string? Description, TaskItemStatus Status, int? ProjectId);
public record UpdateTaskRequest(string Title, string? Description, TaskItemStatus Status, int? ProjectId);
