namespace AutoQ.Agent.Core.Tools;

using AutoQ.Agent.Core.Domain;

public interface ICompensationCoordinator
{
    Task RunAsync(JobState state, IReadOnlyList<SpecialistKind> executedSteps, CancellationToken ct);
}
