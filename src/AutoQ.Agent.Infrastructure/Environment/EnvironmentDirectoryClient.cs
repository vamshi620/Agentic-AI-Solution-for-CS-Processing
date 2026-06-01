namespace AutoQ.Agent.Infrastructure.Environment;

using System.Net.Http.Json;
using AutoQ.Agent.Core.Options;
using Microsoft.Extensions.Options;

public sealed class EnvironmentDirectoryClient
{
    private readonly HttpClient _httpClient;
    private readonly EnvironmentDirectoryOptions _options;

    public EnvironmentDirectoryClient(HttpClient httpClient, IOptions<EnvironmentDirectoryOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<EnvironmentDetails> GetEnvironmentAsync(int environmentId, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/environments/{environmentId}");
        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            request.Headers.TryAddWithoutValidation("x-api-key", _options.ApiKey);
        }

        using var response = await _httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        var details = await response.Content.ReadFromJsonAsync<EnvironmentDetails>(cancellationToken: ct);
        return details ?? throw new InvalidOperationException("Environment directory returned empty response.");
    }
}
