namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IEnvironmentResolverTool
{
    Task<EnvironmentResolution> ResolveAsync(int environmentId, string principalId, string jobId, CancellationToken ct);
}
