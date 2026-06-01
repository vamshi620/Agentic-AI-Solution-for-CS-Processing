namespace AutoQ.Agent.Infrastructure.Queues;

using System.Collections.Concurrent;

public interface IQueueStore
{
    int Enqueue(string solutionId, IReadOnlyList<long> claimIds);
    IReadOnlyList<long> Peek(string solutionId, int max);
    int Depth(string solutionId);
}

public sealed class InMemoryQueueStore : IQueueStore
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<long>> _queues = new();

    public int Enqueue(string solutionId, IReadOnlyList<long> claimIds)
    {
        var queue = _queues.GetOrAdd(solutionId, _ => new ConcurrentQueue<long>());
        var enqueued = 0;
        foreach (var id in claimIds)
        {
            queue.Enqueue(id);
            enqueued++;
        }
        return enqueued;
    }

    public IReadOnlyList<long> Peek(string solutionId, int max)
    {
        if (!_queues.TryGetValue(solutionId, out var queue))
        {
            return Array.Empty<long>();
        }

        return queue.Take(max).ToArray();
    }

    public int Depth(string solutionId) =>
        _queues.TryGetValue(solutionId, out var queue) ? queue.Count : 0;
}
