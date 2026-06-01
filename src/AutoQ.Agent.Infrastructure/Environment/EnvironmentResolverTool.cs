namespace AutoQ.Agent.Infrastructure.Environment;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Tools;

public sealed class EnvironmentResolverTool : IEnvironmentResolverTool
{
    private readonly EnvironmentDirectoryClient _client;
    private readonly IEnvironmentTokenStore _tokenStore;

    public EnvironmentResolverTool(EnvironmentDirectoryClient client, IEnvironmentTokenStore tokenStore)
    {
        _client = client;
        _tokenStore = tokenStore;
    }

    public async Task<EnvironmentResolution> ResolveAsync(int environmentId, string principalId, string jobId, CancellationToken ct)
    {
        var details = await _client.GetEnvironmentAsync(environmentId, ct);
        var handle = _tokenStore.Store(details);
        return new EnvironmentResolution
        {
            EnvironmentId = details.EnvironmentId,
            EnvironmentCode = details.EnvironmentCode,
            TokenHandle = handle,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(10)
        };
    }
}
