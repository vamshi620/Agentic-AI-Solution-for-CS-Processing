namespace AutoQ.Agent.Core.Domain;

public sealed record CustomSolutionDefinition
{
    public required string SolutionId { get; init; }
    public required string ProcessingStoredProcedure { get; init; }
    public IReadOnlyList<SelectionVariant> SelectionVariants { get; init; } = Array.Empty<SelectionVariant>();
}

public sealed record SelectionVariant
{
    public required string Variant { get; init; }
    public required string StoredProcedure { get; init; }
    public string? Description { get; init; }
}
