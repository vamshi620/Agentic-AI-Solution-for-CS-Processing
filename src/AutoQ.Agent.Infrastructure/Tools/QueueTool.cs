namespace AutoQ.Agent.Infrastructure.Tools;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;
using AutoQ.Agent.Infrastructure.Queues;

public sealed class QueueTool : IQueueTool
{
    private readonly IQueueStore _store;

    public QueueTool(IQueueStore store)
    {
        _store = store;
    }

    public Task<EnqueueResult> EnqueueAsync(
        string customSolutionId,
        string environmentTokenHandle,
        IReadOnlyList<long> claimIds,
        string idempotencyKey,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var enqueued = _store.Enqueue(customSolutionId, claimIds);
        var depth = _store.Depth(customSolutionId);
        return Task.FromResult(new EnqueueResult
        {
            Enqueued = enqueued,
            Skipped = Math.Max(0, claimIds.Count - enqueued),
            Depth = depth
        });
    }

    public Task<QueueSnapshot> PeekAsync(string customSolutionId, string environmentTokenHandle, int max, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var claims = _store.Peek(customSolutionId, max);
        return Task.FromResult(new QueueSnapshot
        {
            Depth = _store.Depth(customSolutionId),
            NextClaims = claims
        });
    }
}
