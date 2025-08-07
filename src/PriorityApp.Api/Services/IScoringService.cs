using PriorityApp.PriorityApp.Shared;

namespace PriorityApp.PriorityApp.Api.Services;

public interface IScoringService
{
    decimal CalculateScore(TaskItem task, UserPreferences pref);
}