namespace AutoQ.Agent.Infrastructure.StoredProcedures;

using System.Data;
using Microsoft.Data.SqlClient;

public sealed class SqlStoredProcedureExecutor : IStoredProcedureExecutor
{
    public async Task<StoredProcedureResult> ExecuteAsync(
        string connectionString,
        string storedProcedure,
        IReadOnlyDictionary<string, object?> parameters,
        CancellationToken ct)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand(storedProcedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        foreach (var (key, value) in parameters)
        {
            command.Parameters.AddWithValue(key, value ?? DBNull.Value);
        }

        await using var reader = await command.ExecuteReaderAsync(ct);
        var rows = new List<IReadOnlyDictionary<string, object?>>();
        while (await reader.ReadAsync(ct))
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < reader.FieldCount; i++)
            {
                dict[reader.GetName(i)] = await reader.IsDBNullAsync(i, ct) ? null : reader.GetValue(i);
            }
            rows.Add(dict);
        }

        return new StoredProcedureResult(rows);
    }
}
