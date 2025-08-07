using PriorityApp.Shared;

namespace PriorityApp.Api.Services;

public interface IScoringService
{
    decimal CalculateScore(TaskItem task, UserPreferences pref);
}