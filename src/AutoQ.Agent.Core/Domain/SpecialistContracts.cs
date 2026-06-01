namespace AutoQ.Agent.Core.Domain;

public enum SpecialistKind
{
    EnvironmentResolver,
    SelectionStrategist,
    QueueManager,
    Processing,
    ComplianceAudit,
    HitlLiaison
}

public sealed record SpecialistContext
{
    public required string JobId { get; init; }
    public required string CorrelationId { get; init; }
    public required string CustomSolutionId { get; init; }
    public required string PrincipalId { get; init; }
    public required int EnvironmentId { get; init; }
    public string? EnvironmentTokenHandle { get; init; }
    public IReadOnlyDictionary<string, string> Parameters { get; init; } = new Dictionary<string, string>();
    public int BatchSize { get; init; } = 100;
}

public sealed record SpecialistResult
{
    public required SpecialistKind Kind { get; init; }
    public required bool Success { get; init; }
    public string? Summary { get; init; }
    public string? Error { get; init; }
    public Dictionary<string, string> Outputs { get; init; } = new();
    public List<string> AuditEventIds { get; init; } = new();
    public bool RequiresHumanApproval { get; init; }
    public string? HitlTicketId { get; init; }
    public object? Data { get; init; }
}
