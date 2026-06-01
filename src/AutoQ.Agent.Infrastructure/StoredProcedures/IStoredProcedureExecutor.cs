namespace AutoQ.Agent.Infrastructure.StoredProcedures;

public interface IStoredProcedureExecutor
{
    Task<StoredProcedureResult> ExecuteAsync(
        string connectionString,
        string storedProcedure,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken ct);
}
