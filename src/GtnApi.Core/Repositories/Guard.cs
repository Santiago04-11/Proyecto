using GtnApi.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Repositories;

/// <summary>Validaciones compartidas por los repositorios.</summary>
internal static class Guard
{
    public static string Required(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} es obligatorio.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new ArgumentException($"{fieldName} no puede superar {maxLength} caracteres.");
        }

        return trimmed;
    }

    public static string? Optional(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
        {
            throw new ArgumentException($"{fieldName} no puede superar {maxLength} caracteres.");
        }

        return trimmed;
    }

    public static async Task EnsureProjectExistsAsync(GtnDbContext ctx, int? projectId, CancellationToken ct)
    {
        if (projectId is null)
        {
            return;
        }

        var exists = await ctx.Projects.AnyAsync(p => p.Id == projectId, ct);
        if (!exists)
        {
            throw new ArgumentException($"El proyecto con id {projectId} no existe.");
        }
    }
}
