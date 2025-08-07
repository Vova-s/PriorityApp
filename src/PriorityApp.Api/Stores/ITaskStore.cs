namespace PriorityApp.PriorityApp.Api.Stores;

using PriorityApp.Shared;

public interface ITaskStore
{
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(Guid userId);
    Task<TaskItem?> GetAsync(Guid userId, Guid taskId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid userId, Guid taskId);
}