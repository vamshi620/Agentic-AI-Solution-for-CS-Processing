namespace AutoQ.Agent.Infrastructure.Configuration;

using AutoQ.Agent.Core.Domain;
using AutoQ.Agent.Core.Options;
using Microsoft.Extensions.Options;

public sealed class CustomSolutionRegistry
{
    private readonly Dictionary<string, CustomSolutionDefinition> _solutions;

    public CustomSolutionRegistry(IOptions<CustomSolutionOptions> options)
    {
        _solutions = options.Value.Solutions
            .ToDictionary(s => s.SolutionId, StringComparer.OrdinalIgnoreCase);
    }

    public CustomSolutionDefinition Get(string solutionId)
    {
        if (_solutions.TryGetValue(solutionId, out var solution))
        {
            return solution;
        }

        throw new KeyNotFoundException($"Unknown custom solution '{solutionId}'.");
    }
}
