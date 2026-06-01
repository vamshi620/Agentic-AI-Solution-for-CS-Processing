namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IHitlTool
{
    Task<HitlTicket> RequestApprovalAsync(HitlRequest request, CancellationToken ct);
}
