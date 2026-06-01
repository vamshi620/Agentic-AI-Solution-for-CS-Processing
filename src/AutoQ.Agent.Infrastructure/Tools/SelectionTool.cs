namespace AutoQ.Agent.Infrastructure.Tools;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;
using AutoQ.Agent.Infrastructure.Configuration;
using AutoQ.Agent.Infrastructure.Environment;
using AutoQ.Agent.Infrastructure.StoredProcedures;

public sealed class SelectionTool : ISelectionTool
{
    private readonly CustomSolutionRegistry _solutions;
    private readonly IEnvironmentConnectionProvider _connectionProvider;
    private readonly IStoredProcedureExecutor _executor;

    public SelectionTool(
        CustomSolutionRegistry solutions,
        IEnvironmentConnectionProvider connectionProvider,
        IStoredProcedureExecutor executor)
    {
        _solutions = solutions;
        _connectionProvider = connectionProvider;
        _executor = executor;
    }

    public async Task<SelectionResult> RunSelectionAsync(
        string customSolutionId,
        string variant,
        string environmentTokenHandle,
        string idempotencyKey,
        IReadOnlyDictionary<string, string> parameters,
        CancellationToken ct)
    {
        var solution = _solutions.Get(customSolutionId);
        var selectedVariant = solution.SelectionVariants
            .FirstOrDefault(v => v.Variant.Equals(variant, StringComparison.OrdinalIgnoreCase))
            ?? solution.SelectionVariants.First();

        var connectionString = _connectionProvider.GetConnectionString(environmentTokenHandle);
        var execId = Guid.NewGuid().ToString("n");
        var result = await _executor.ExecuteAsync(
            connectionString,
            selectedVariant.StoredProcedure,
            parameters.ToDictionary(k => k.Key, v => (object?)v.Value),
            ct);

        var claimIds = ExtractClaimIds(result);
        return new SelectionResult
        {
            SelectedCount = claimIds.Count,
            ExecutionId = execId,
            Variant = selectedVariant.Variant,
            ClaimIds = claimIds
        };
    }

    private static IReadOnlyList<long> ExtractClaimIds(StoredProcedureResult result)
    {
        var ids = new List<long>();
        foreach (var row in result.Rows)
        {
            if (row.TryGetValue("ClaimId", out var value) && value is not null && long.TryParse(value.ToString(), out var id))
            {
                ids.Add(id);
                continue;
            }
            if (row.TryGetValue("claim_id", out value) && value is not null && long.TryParse(value.ToString(), out id))
            {
                ids.Add(id);
            }
        }
        return ids;
    }
}
