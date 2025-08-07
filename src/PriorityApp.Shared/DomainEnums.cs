namespace PriorityApp.Shared;

public enum DomainType { Career, Life }

public enum PgLevel
{
    Direct = 100,
    Indirect = 85,
    Neutral = 50,
    DragIndirect = 25,
    DragDirect = 0
}

public enum DependencyLevel
{
    FamilyColleagues = 100,
    Customer = 50,
    None = 0
}

public enum ImpactLevel
{
    Negative = 100,
    Positive = 50,
    Neutral = 0
}