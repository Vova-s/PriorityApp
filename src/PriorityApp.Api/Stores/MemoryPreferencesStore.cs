using System.Collections.Concurrent;
using PriorityApp.PriorityApp.Api.Stores;
using PriorityApp.PriorityApp.Shared;



namespace PriorityApp.Api.Stores;

public class MemoryPreferencesStore : IPreferencesStore
{
    // Ключ – UserId, значення – об’єкт налаштувань
    private readonly ConcurrentDictionary<Guid, UserPreferences> _store = new();

    public Task<UserPreferences?> GetAsync(Guid userId) =>
        Task.FromResult(_store.TryGetValue(userId, out var pref) ? pref : null);

    public Task SetAsync(UserPreferences pref)
    {
        _store[pref.UserId] = pref;
        return Task.CompletedTask;
    }
}