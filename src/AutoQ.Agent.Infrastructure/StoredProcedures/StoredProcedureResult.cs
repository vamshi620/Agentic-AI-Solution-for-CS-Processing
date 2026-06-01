namespace AutoQ.Agent.Infrastructure.StoredProcedures;

public sealed record StoredProcedureResult(IReadOnlyList<IReadOnlyDictionary<string, object?>> Rows);
