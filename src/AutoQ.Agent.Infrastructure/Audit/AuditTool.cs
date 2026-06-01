namespace AutoQ.Agent.Infrastructure.Audit;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Security;
using AutoQ.Agent.Core.Tools;

public sealed class AuditTool : IAuditTool
{
    private readonly IAuditChainStore _store;

    public AuditTool(IAuditChainStore store)
    {
        _store = store;
    }

    public async Task<string> LogAsync(AuditEntry entry, CancellationToken ct)
    {
        var record = await _store.AppendAsync(entry, ct);
        return record.EventId;
    }
}
