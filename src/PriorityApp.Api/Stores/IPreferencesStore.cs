namespace PriorityApp.PriorityApp.Api.Stores;

using PriorityApp.Shared;

public interface IPreferencesStore
{
    Task<UserPreferences?> GetAsync(Guid userId);
    Task SetAsync(UserPreferences pref);
}