using PriorityApp.Shared;

namespace PriorityApp.Api.Services;

public class ScoringService : IScoringService
{
    public decimal CalculateScore(TaskItem task, UserPreferences pref)
    {
        // ----- 1️⃣ domain weight -------------------------------------------------
        var domainWeight = task.Domain == DomainType.Career ? pref.CareerWeight : pref.LifeWeight;

        // ----- 2️⃣ PG factor -----------------------------------------------------
        var pgFactor = ((int)task.Pg) / 100m;

        // ----- 3️⃣ normalised criteria -----------------------------------------
        decimal effort = task.EstimatedHours <= 2 ? 1m :
                         task.EstimatedHours <= 5 ? 0.5m : 0m;

        decimal dependency = ((int)task.Dependency) / 100m;
        decimal impact     = ((int)task.Impact) / 100m;

        // ----- 4️⃣ urgency -------------------------------------------------------
        var now = DateTime.UtcNow;
        decimal urgency;
        if (task.DueDate < now) urgency = 1m;
        else if (task.DueDate <= now.AddHours((double)task.EstimatedHours)) urgency = 0.5m;
        else if (task.DueDate > now.AddHours((double)task.EstimatedHours * 2)) urgency = 0m;
        else
        {
            // linear interpolation between 0.5 and 0
            var diff   = (task.DueDate - now.AddHours((double)task.EstimatedHours)).TotalHours;
            var range  = task.EstimatedHours; // from 1*est to 2*est
            urgency = 0.5m * (1 - (decimal)(diff / (double)range));
        }

        // ----- 5️⃣ composite (inner) weights ------------------------------------
        var sum = task.WeightEffort + task.WeightDependency + task.WeightImpact + task.WeightUrgency;
        if (sum == 0) throw new InvalidOperationException("All inner weights are zero.");
        var norm = 1m / sum;

        var composite = task.WeightEffort      * norm * effort
                      + task.WeightDependency * norm * dependency
                      + task.WeightImpact     * norm * impact
                      + task.WeightUrgency    * norm * urgency;

        // ----- 6️⃣ final score ---------------------------------------------------
        var total = domainWeight * (pref.Beta1 * pgFactor + pref.Beta2 * composite);
        return Math.Clamp(total, 0m, 1m);
    }
}
