namespace AutoQ.Agent.Core.Options;

using AutoQ.Agent.Core.Domain;
using Microsoft.Extensions.AI;

public sealed class AgentExecutionOptions
{
    public bool UseAi { get; set; } = true;
    public string SmallModelId { get; set; } = "gpt-4o-mini";
    public string LargeModelId { get; set; } = "gpt-4o";
    public Dictionary<SpecialistKind, AgentBudget> Budgets { get; set; } = new();

    public AgentBudget GetBudget(SpecialistKind kind) =>
        Budgets.TryGetValue(kind, out var budget)
            ? budget
            : AgentBudget.Default(kind);
}

public sealed class AgentBudget
{
    public int MaxOutputTokens { get; set; } = 600;
    public bool AllowMultipleToolCalls { get; set; }
    public ReasoningEffort ReasoningEffort { get; set; } = ReasoningEffort.Low;
    public string? ModelIdOverride { get; set; }

    public static AgentBudget Default(SpecialistKind kind) => kind switch
    {
        SpecialistKind.Processing => new AgentBudget { MaxOutputTokens = 1000, ReasoningEffort = ReasoningEffort.High },
        SpecialistKind.ComplianceAudit => new AgentBudget { MaxOutputTokens = 800, ReasoningEffort = ReasoningEffort.Medium },
        _ => new AgentBudget { MaxOutputTokens = 400, ReasoningEffort = ReasoningEffort.Low }
    };
}
