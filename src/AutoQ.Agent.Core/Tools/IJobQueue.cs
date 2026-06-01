namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IJobQueue
{
    ValueTask EnqueueAsync(JobTicket ticket, CancellationToken ct);
    IAsyncEnumerable<JobTicket> ReadAllAsync(CancellationToken ct);
}
