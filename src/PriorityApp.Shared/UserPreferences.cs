namespace PriorityApp.Shared;

public class UserPreferences
{
    public Guid UserId { get; set; }
    public decimal CareerWeight { get; set; }   // 0..1
    public decimal LifeWeight   { get; set; }   // 0..1  (CareerWeight + LifeWeight == 1)
    public decimal Beta1 { get; set; } = 0.6M; // weight for PG factor
    public decimal Beta2 { get; set; } = 0.4M; // weight for inner criteria
    public DateTime UpdatedAt { get; set; }
}