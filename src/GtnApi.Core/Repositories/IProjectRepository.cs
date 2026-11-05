using GtnApi.Core.Models;

namespace GtnApi.Core.Repositories;

public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default);
    Task<Project?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Project> AddAsync(Project project, CancellationToken ct = default);

    /// <returns>false si el proyecto no existe.</returns>
    Task<bool> UpdateAsync(Project project, CancellationToken ct = default);

    /// <summary>Elimina el proyecto; sus notas y tareas quedan sin proyecto.</summary>
    /// <returns>false si el proyecto no existe.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
