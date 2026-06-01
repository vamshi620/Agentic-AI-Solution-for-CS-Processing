namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IQueueTool
{
    Task<EnqueueResult> EnqueueAsync(
        string customSolutionId,
        string environmentTokenHandle,
        IReadOnlyList<long> claimIds,
        string idempotencyKey,
        CancellationToken ct);

    Task<QueueSnapshot> PeekAsync(string customSolutionId, string environmentTokenHandle, int max, CancellationToken ct);
}
