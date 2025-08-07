using System.Collections.Concurrent;
using PriorityApp.Api.Stores;
using PriorityApp.Shared;

namespace PriorityApp.Api.Stores;

public class MemoryTaskStore : ITaskStore
{
    // composite key (userId, taskId) → TaskItem
    private readonly ConcurrentDictionary<(Guid user, Guid task), TaskItem> _store = new();

    public Task<IReadOnlyCollection<TaskItem>> GetAllAsync(Guid userId) =>
        Task.FromResult<IReadOnlyCollection<TaskItem>>(
            _store.Values.Where(t => t.UserId == userId).ToList());

    public Task<TaskItem?> GetAsync(Guid userId, Guid taskId) =>
        Task.FromResult(_store.GetValueOrDefault((userId, taskId)));

    public Task AddAsync(TaskItem task)
    {
        _store[(task.UserId, task.Id)] = task;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TaskItem task)
    {
        _store[(task.UserId, task.Id)] = task;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid userId, Guid taskId)
    {
        _store.TryRemove((userId, taskId), out _);
        return Task.CompletedTask;
    }
}