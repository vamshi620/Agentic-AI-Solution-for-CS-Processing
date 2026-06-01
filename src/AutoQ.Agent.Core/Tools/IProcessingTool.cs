namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface IProcessingTool
{
    Task<ProcessingBatchResult> RunProcessingAsync(
        string customSolutionId,
        string environmentTokenHandle,
        IReadOnlyList<long> claimIds,
        string idempotencyKey,
        CancellationToken ct);
}
