namespace AutoQ.Agent.Core.Domain;

public enum JobStatus
{
    Created,
    Running,
    RequiresHumanReview,
    Completed,
    Failed
}

public sealed record JobTicket
{
    public required string JobId { get; init; }
    public required string CorrelationId { get; init; }
    public required string CustomSolutionId { get; init; }
    public required int EnvironmentId { get; init; }
    public required string PrincipalId { get; init; }
    public IReadOnlyDictionary<string, string> Parameters { get; init; } = new Dictionary<string, string>();
    public DateTimeOffset RequestedAt { get; init; } = DateTimeOffset.UtcNow;
}

public sealed record JobState
{
    public required string JobId { get; init; }
    public required string CorrelationId { get; init; }
    public required string CustomSolutionId { get; init; }
    public required int EnvironmentId { get; init; }
    public required string PrincipalId { get; init; }
    public JobStatus Status { get; set; } = JobStatus.Created;
    public string? EnvironmentTokenHandle { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<SpecialistKind> CompletedSteps { get; init; } = new();
    public List<string> AuditEventIds { get; init; } = new();
    public List<long> SelectedClaimIds { get; init; } = new();
    public int QueueDepth { get; set; }
    public int SelectedCount { get; set; }
    public int ProcessedCount { get; set; }
    public int PendedCount { get; set; }
    public int FailedCount { get; set; }
    public bool RequiresHumanApproval { get; set; }
    public string? HitlTicketId { get; set; }
    public string? FailureReason { get; set; }
}
