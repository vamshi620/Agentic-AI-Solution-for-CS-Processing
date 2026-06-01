namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IAuditTool
{
    Task<string> LogAsync(AuditEntry entry, CancellationToken ct);
}
