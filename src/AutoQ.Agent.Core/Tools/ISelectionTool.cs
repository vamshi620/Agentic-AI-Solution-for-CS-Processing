namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface ISelectionTool
{
    Task<SelectionResult> RunSelectionAsync(
        string customSolutionId,
        string environmentTokenHandle,
        string idempotencyKey,
        IReadOnlyDictionary<string, string> parameters,
        CancellationToken ct);
}
