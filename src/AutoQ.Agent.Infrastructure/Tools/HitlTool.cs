namespace AutoQ.Agent.Infrastructure.Tools;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;

public sealed class HitlTool : IHitlTool
{
    public Task<HitlTicket> RequestApprovalAsync(HitlRequest request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var ticket = new HitlTicket
        {
            TicketId = $"HITL-{Guid.NewGuid():N}",
            RiskLevel = request.RiskLevel
        };
        return Task.FromResult(ticket);
    }
}
