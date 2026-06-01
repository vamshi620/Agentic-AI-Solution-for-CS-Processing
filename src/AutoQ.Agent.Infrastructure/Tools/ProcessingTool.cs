namespace AutoQ.Agent.Infrastructure.Tools;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;
using AutoQ.Agent.Infrastructure.Configuration;
using AutoQ.Agent.Infrastructure.Environment;
using AutoQ.Agent.Infrastructure.StoredProcedures;

public sealed class ProcessingTool : IProcessingTool
{
    private readonly CustomSolutionRegistry _solutions;
    private readonly IEnvironmentConnectionProvider _connectionProvider;
    private readonly IStoredProcedureExecutor _executor;

    public ProcessingTool(
        CustomSolutionRegistry solutions,
        IEnvironmentConnectionProvider connectionProvider,
        IStoredProcedureExecutor executor)
    {
        _solutions = solutions;
        _connectionProvider = connectionProvider;
        _executor = executor;
    }

    public async Task<ProcessingBatchResult> RunProcessingAsync(
        string customSolutionId,
        string environmentTokenHandle,
        IReadOnlyList<long> claimIds,
        string idempotencyKey,
        CancellationToken ct)
    {
        var solution = _solutions.Get(customSolutionId);
        var connectionString = _connectionProvider.GetConnectionString(environmentTokenHandle);
        var parameters = new Dictionary<string, object?>
        {
            ["@ClaimIds"] = string.Join(',', claimIds)
        };

        var result = await _executor.ExecuteAsync(
            connectionString,
            solution.ProcessingStoredProcedure,
            parameters,
            ct);

        return Summarize(result, claimIds.Count);
    }

    private static ProcessingBatchResult Summarize(StoredProcedureResult result, int fallbackCount)
    {
        var processed = 0;
        var pended = 0;
        var failed = 0;
        var pendCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in result.Rows)
        {
            var status = row.TryGetValue("status", out var value) ? value?.ToString() : null;
            var pend = row.TryGetValue("pend_code", out var pendValue) ? pendValue?.ToString() : null;

            switch (status?.ToLowerInvariant())
            {
                case "pended":
                    pended++;
                    if (!string.IsNullOrWhiteSpace(pend))
                    {
                        pendCodes.Add(pend);
                    }
                    break;
                case "failed":
                    failed++;
                    break;
                default:
                    processed++;
                    break;
            }
        }

        if (result.Rows.Count == 0)
        {
            processed = fallbackCount;
        }

        return new ProcessingBatchResult
        {
            Processed = processed,
            Pended = pended,
            Failed = failed,
            PendCodes = pendCodes.ToArray()
        };
    }
}
