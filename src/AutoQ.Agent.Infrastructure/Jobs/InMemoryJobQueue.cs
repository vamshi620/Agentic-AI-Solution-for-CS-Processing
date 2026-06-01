namespace AutoQ.Agent.Infrastructure.Jobs;

using System.Threading.Channels;
using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;

public sealed class InMemoryJobQueue : IJobQueue
{
    private readonly Channel<JobTicket> _channel = Channel.CreateUnbounded<JobTicket>();

    public ValueTask EnqueueAsync(JobTicket ticket, CancellationToken ct) =>
        _channel.Writer.WriteAsync(ticket, ct);

    public IAsyncEnumerable<JobTicket> ReadAllAsync(CancellationToken ct) =>
        _channel.Reader.ReadAllAsync(ct);
}
