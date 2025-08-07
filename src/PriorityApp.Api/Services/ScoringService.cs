using PriorityApp.PriorityApp.Shared;

namespace PriorityApp.PriorityApp.Api.Services;

public class ScoringService : IScoringService
{
    public decimal CalculateScore(TaskItem task, UserPreferences pref)
    {
        // 1. domain weight
        var domainWeight = task.Domain == DomainType.Career ? pref.CareerWeight : pref.LifeWeight;

        // 2. PG factor
        var pgFactor = ((int)task.Pg) / 100m;

        // 3. Normalised criteria
        decimal effort = task.EstimatedHours <= 2 ? 1m :
            task.EstimatedHours <= 5 ? 0.5m : 0m;

        decimal dependency = ((int)task.Dependency) / 100m;
        decimal impact = ((int)task.Impact) / 100m;

        // urgency
        var now = DateTime.UtcNow;
        decimal urgency;
        if (task.DueDate < now) urgency = 1m;
        else if (task.DueDate <= now.AddHours((double)task.EstimatedHours)) urgency = 0.5m;
        else if (task.DueDate > now.AddHours((double)task.EstimatedHours * 2)) urgency = 0m;
        else
        {
            // linear between 0.5 and 0
            var diff = (task.DueDate - now.AddHours((double)task.EstimatedHours)).TotalHours;
            var range = task.EstimatedHours; // from 1*est to 2*est
            urgency = 0.5m * (1 - (decimal)(diff / (double)range));
        }

        // 4. Composite (weights must sum 1 – we trust UI, but clamp)
        var alphaSum = task.WeightEffort + task.WeightDependency + task.WeightImpact + task.WeightUrgency;
        if (alphaSum == 0) throw new InvalidOperationException("All inner weights are zero.");
        var norm = 1m / alphaSum;
        var composite = task.WeightEffort * norm * effort
                        + task.WeightDependency * norm * dependency
                        + task.WeightImpact * norm * impact
                        + task.WeightUrgency * norm * urgency;

        // 5. Final score
        var total = domainWeight * (pref.Beta1 * pgFactor + pref.Beta2 * composite);
        // clamp 0..1
        return Math.Clamp(total, 0m, 1m);
    }
}