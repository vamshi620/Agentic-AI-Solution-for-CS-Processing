namespace AutoQ.Agent.Infrastructure.Environment;

using System.Security.Cryptography;
using AutoQ.Agent.Core.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

public interface IEnvironmentTokenStore
{
    string Store(EnvironmentDetails details);
    EnvironmentDetails? Resolve(string tokenHandle);
}

public sealed class EnvironmentTokenStore : IEnvironmentTokenStore
{
    private readonly IMemoryCache _cache;
    private readonly EnvironmentDirectoryOptions _options;

    public EnvironmentTokenStore(IMemoryCache cache, IOptions<EnvironmentDirectoryOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public string Store(EnvironmentDetails details)
    {
        var handle = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
        _cache.Set(handle, details, _options.CacheTtl);
        return handle;
    }

    public EnvironmentDetails? Resolve(string tokenHandle) =>
        _cache.TryGetValue(tokenHandle, out EnvironmentDetails? details) ? details : null;
}
