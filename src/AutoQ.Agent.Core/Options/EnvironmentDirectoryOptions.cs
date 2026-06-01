namespace AutoQ.Agent.Core.Options;

public sealed class EnvironmentDirectoryOptions
{
    public string BaseUrl { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public TimeSpan CacheTtl { get; set; } = TimeSpan.FromMinutes(10);
}
