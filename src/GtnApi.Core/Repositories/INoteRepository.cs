using GtnApi.Core.Models;

namespace GtnApi.Core.Repositories;

public interface INoteRepository
{
    Task<IReadOnlyList<Note>> GetAllAsync(int? projectId = null, CancellationToken ct = default);
    Task<Note?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Note> AddAsync(Note note, CancellationToken ct = default);

    /// <returns>false si la nota no existe.</returns>
    Task<bool> UpdateAsync(Note note, CancellationToken ct = default);

    /// <returns>false si la nota no existe.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
