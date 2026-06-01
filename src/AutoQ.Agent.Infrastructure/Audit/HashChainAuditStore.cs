namespace AutoQ.Agent.Infrastructure.Audit;

using System.Collections.Concurrent;
using System.Text.Json;
using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Security;

public sealed class HashChainAuditStore : IAuditChainStore
{
    private readonly ConcurrentDictionary<string, List<AuditRecord>> _records = new();
    private readonly IAuditSigner _signer;

    public HashChainAuditStore(IAuditSigner signer)
    {
        _signer = signer;
    }

    public Task<AuditRecord> AppendAsync(AuditEntry entry, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var list = _records.GetOrAdd(entry.JobId, _ => new List<AuditRecord>());
        lock (list)
        {
            var previousHash = list.LastOrDefault()?.Hash;
            var payload = JsonSerializer.Serialize(entry) + "|" + previousHash;
            var hash = _signer.Sign(payload);
            var record = new AuditRecord
            {
                EventId = Guid.NewGuid().ToString("n"),
                JobId = entry.JobId,
                EventType = entry.EventType,
                Hash = hash,
                PreviousHash = previousHash,
                OccurredAt = entry.OccurredAt,
                Attributes = new Dictionary<string, string>(entry.Attributes)
            };
            list.Add(record);
            return Task.FromResult(record);
        }
    }

    public Task<string?> GetLastHashAsync(string jobId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (_records.TryGetValue(jobId, out var list) && list.Count > 0)
        {
            return Task.FromResult<string?>(list[^1].Hash);
        }
        return Task.FromResult<string?>(null);
    }

    public Task<IReadOnlyList<AuditRecord>> GetChainAsync(string jobId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (_records.TryGetValue(jobId, out var list))
        {
            return Task.FromResult<IReadOnlyList<AuditRecord>>(list.ToArray());
        }
        return Task.FromResult<IReadOnlyList<AuditRecord>>(Array.Empty<AuditRecord>());
    }
}
