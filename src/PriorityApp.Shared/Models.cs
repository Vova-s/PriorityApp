namespace PriorityApp.PriorityApp.Shared;

public class UserPreferences
{
    public Guid UserId { get; set; }
    public decimal CareerWeight { get; set; }   // 0..1
    public decimal LifeWeight   { get; set; }   // 0..1, CareerWeight+LifeWeight = 1
    public decimal Beta1 { get; set; } = 0.6M;  // weight for PG
    public decimal Beta2 { get; set; } = 0.4M;  // weight for inner criteria
    public DateTime UpdatedAt { get; set; }
}

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;

    public DomainType Domain { get; set; }
    public DateTime DueDate { get; set; }
    public decimal EstimatedHours { get; set; }

    // 5‑рівневі оцінки
    public PgLevel Pg { get; set; }
    public DependencyLevel Dependency { get; set; }
    public ImpactLevel Impact { get; set; }

    // Внутрішнє розподілення 100 % між 4‑х критеріїв
    public decimal WeightEffort   { get; set; }   // αE
    public decimal WeightDependency { get; set; } // αD
    public decimal WeightImpact   { get; set; }   // αI
    public decimal WeightUrgency  { get; set; }   // αU
    // Σα = 1 – валідація в сервісі

    public decimal ComputedScore { get; set; }    // 0..1 – результат формули
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}