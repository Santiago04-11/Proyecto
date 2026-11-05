namespace GtnApi.Core.Models;

/// <summary>Texto breve para recordar una idea, snippet o bug.</summary>
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    /// <summary>Proyecto asociado (opcional).</summary>
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
