namespace GtnApi.Core.Models;

/// <summary>Contenedor lógico para agrupar notas y tareas.</summary>
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Note> Notes { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new();
}
