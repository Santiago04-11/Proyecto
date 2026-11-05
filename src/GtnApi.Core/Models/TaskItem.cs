namespace GtnApi.Core.Models;

/// <summary>Unidad de trabajo con estado, opcionalmente asociada a un proyecto.</summary>
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    /// <summary>Proyecto asociado (opcional).</summary>
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
