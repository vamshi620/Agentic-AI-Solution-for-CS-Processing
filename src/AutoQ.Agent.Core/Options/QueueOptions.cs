namespace AutoQ.Agent.Core.Options;

public sealed class QueueOptions
{
    public int MaxBatchSize { get; set; } = 200;
    public int MaxParallelJobs { get; set; } = 4;
}
