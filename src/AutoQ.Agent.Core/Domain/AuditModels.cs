namespace AutoQ.Agent.Core.Domain;

public sealed record AuditEntry
{
    public required string JobId { get; init; }
    public required string EventType { get; init; }
    public required string PrincipalId { get; init; }
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    public Dictionary<string, string> Attributes { get; init; } = new();
}

public sealed record AuditRecord
{
    public required string EventId { get; init; }
    public required string JobId { get; init; }
    public required string EventType { get; init; }
    public required string Hash { get; init; }
    public string? PreviousHash { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public Dictionary<string, string> Attributes { get; init; } = new();
}
