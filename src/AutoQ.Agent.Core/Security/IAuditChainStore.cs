namespace AutoQ.Agent.Core.Security;

using AutoQ.Agent.Core.Domain;

public interface IAuditChainStore
{
    Task<AuditRecord> AppendAsync(AuditEntry entry, CancellationToken ct);
    Task<string?> GetLastHashAsync(string jobId, CancellationToken ct);
    Task<IReadOnlyList<AuditRecord>> GetChainAsync(string jobId, CancellationToken ct);
}
