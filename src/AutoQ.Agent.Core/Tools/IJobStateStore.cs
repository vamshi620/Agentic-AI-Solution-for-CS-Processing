namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IJobStateStore
{
    Task<JobState?> GetAsync(string jobId, CancellationToken ct);
    Task SaveAsync(JobState state, CancellationToken ct);
}
