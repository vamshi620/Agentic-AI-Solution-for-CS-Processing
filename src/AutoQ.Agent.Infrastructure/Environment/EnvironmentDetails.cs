namespace AutoQ.Agent.Infrastructure.Environment;

public sealed record EnvironmentDetails
{
    public required int EnvironmentId { get; init; }
    public required string EnvironmentCode { get; init; }
    public required string ConnectionString { get; init; }
}
