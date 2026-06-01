namespace AutoQ.Agent.Core.Options;

using AutoQ.Agent.Core.Domain;

public sealed class CustomSolutionOptions
{
    public List<CustomSolutionDefinition> Solutions { get; set; } = new();
}
