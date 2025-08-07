namespace PriorityApp.Shared;

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;

    public DomainType Domain { get; set; }
    public DateTime DueDate { get; set; }
    public decimal EstimatedHours { get; set; }

    // 5‑level ratings
    public PgLevel Pg { get; set; }
    public DependencyLevel Dependency { get; set; }
    public ImpactLevel Impact { get; set; }

    // Inner weight distribution (must sum to 1)
    public decimal WeightEffort   { get; set; }   // αE
    public decimal WeightDependency { get; set; } // αD
    public decimal WeightImpact   { get; set; }   // αI
    public decimal WeightUrgency  { get; set; }   // αU

    public decimal ComputedScore { get; set; }    // 0..1 – result of the scoring formula
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}