namespace AutoQ.Agent.Infrastructure.Jobs;

using System.Collections.Concurrent;
using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;

public sealed class InMemoryJobStateStore : IJobStateStore
{
    private readonly ConcurrentDictionary<string, JobState> _states = new();

    public Task<JobState?> GetAsync(string jobId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        _states.TryGetValue(jobId, out var state);
        return Task.FromResult(state);
    }

    public Task SaveAsync(JobState state, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        state.UpdatedAt = DateTimeOffset.UtcNow;
        _states[state.JobId] = state;
        return Task.CompletedTask;
    }
}
