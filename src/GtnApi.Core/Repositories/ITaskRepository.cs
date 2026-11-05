using GtnApi.Core.Models;

namespace GtnApi.Core.Repositories;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(
        int? projectId = null,
        TaskItemStatus? status = null,
        CancellationToken ct = default);

    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default);

    /// <returns>false si la tarea no existe.</returns>
    Task<bool> UpdateAsync(TaskItem task, CancellationToken ct = default);

    /// <returns>false si la tarea no existe.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
