namespace AutoQ.Agent.Core.Domain;

public sealed record EnvironmentResolution
{
    public required int EnvironmentId { get; init; }
    public required string EnvironmentCode { get; init; }
    public required string TokenHandle { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}

public sealed record SelectionResult
{
    public required int SelectedCount { get; init; }
    public required string ExecutionId { get; init; }
    public IReadOnlyList<long> ClaimIds { get; init; } = Array.Empty<long>();
}

public sealed record EnqueueResult
{
    public required int Enqueued { get; init; }
    public required int Skipped { get; init; }
    public required int Depth { get; init; }
}

public sealed record QueueSnapshot
{
    public required int Depth { get; init; }
    public IReadOnlyList<long> NextClaims { get; init; } = Array.Empty<long>();
}

public sealed record ProcessingBatchResult
{
    public required int Processed { get; init; }
    public required int Pended { get; init; }
    public required int Failed { get; init; }
    public IReadOnlyList<string> PendCodes { get; init; } = Array.Empty<string>();
}

public sealed record HitlRequest
{
    public required string JobId { get; init; }
    public required string Reason { get; init; }
    public required string AgentRationale { get; init; }
    public required HitlRiskLevel RiskLevel { get; init; }
    public Dictionary<string, string> Context { get; init; } = new();
}

public sealed record HitlTicket
{
    public required string TicketId { get; init; }
    public required HitlRiskLevel RiskLevel { get; init; }
}

public enum HitlRiskLevel
{
    Low,
    Medium,
    High,
    Critical
}
