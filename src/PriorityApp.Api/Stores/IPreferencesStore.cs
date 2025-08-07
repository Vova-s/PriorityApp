using PriorityApp.Shared;

namespace PriorityApp.Api.Stores;

public interface IPreferencesStore
{
    Task<UserPreferences?> GetAsync(Guid userId);
    Task SetAsync(UserPreferences pref);
}