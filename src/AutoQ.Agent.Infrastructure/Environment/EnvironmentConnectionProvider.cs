namespace AutoQ.Agent.Infrastructure.Environment;

public interface IEnvironmentConnectionProvider
{
    string GetConnectionString(string tokenHandle);
}

public sealed class EnvironmentConnectionProvider : IEnvironmentConnectionProvider
{
    private readonly IEnvironmentTokenStore _tokenStore;

    public EnvironmentConnectionProvider(IEnvironmentTokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    public string GetConnectionString(string tokenHandle)
    {
        var details = _tokenStore.Resolve(tokenHandle)
            ?? throw new InvalidOperationException("Unknown environment token handle.");
        return details.ConnectionString;
    }
}
