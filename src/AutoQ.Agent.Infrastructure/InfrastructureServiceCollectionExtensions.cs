namespace AutoQ.Agent.Infrastructure;

using AutoQ.Agent.Core.Options;
using AutoQ.Agent.Core.Security;
using AutoQ.Agent.Core.Tools;
using AutoQ.Agent.Infrastructure.AI;
using AutoQ.Agent.Infrastructure.Audit;
using AutoQ.Agent.Infrastructure.Configuration;
using AutoQ.Agent.Infrastructure.Environment;
using AutoQ.Agent.Infrastructure.Jobs;
using AutoQ.Agent.Infrastructure.Options;
using AutoQ.Agent.Infrastructure.Queues;
using AutoQ.Agent.Infrastructure.Security;
using AutoQ.Agent.Infrastructure.StoredProcedures;
using AutoQ.Agent.Infrastructure.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddAutoQInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EnvironmentDirectoryOptions>(configuration.GetSection("EnvironmentDirectory"));
        services.Configure<CustomSolutionOptions>(configuration.GetSection("CustomSolutions"));
        services.Configure<AuditSigningOptions>(configuration.GetSection("AuditSigning"));

        services.AddMemoryCache();
        services.AddSingleton<IJobQueue, InMemoryJobQueue>();
        services.AddSingleton<IJobStateStore, InMemoryJobStateStore>();
        services.AddSingleton<IAuditSigner, HmacAuditSigner>();
        services.AddSingleton<IAuditChainStore, HashChainAuditStore>();
        services.AddSingleton<IAuditTool, AuditTool>();
        services.AddSingleton<IPhiSanitizer, RegexPhiSanitizer>();
        services.AddSingleton<IEnvironmentTokenStore, EnvironmentTokenStore>();
        services.AddSingleton<IEnvironmentConnectionProvider, EnvironmentConnectionProvider>();
        services.AddSingleton<IStoredProcedureExecutor, SqlStoredProcedureExecutor>();
        services.AddSingleton<CustomSolutionRegistry>();
        services.AddSingleton<IQueueStore, InMemoryQueueStore>();
        services.AddSingleton<IQueueTool, QueueTool>();
        services.AddSingleton<ISelectionTool, SelectionTool>();
        services.AddSingleton<IProcessingTool, ProcessingTool>();
        services.AddSingleton<IHitlTool, HitlTool>();
        services.TryAddSingleton<IChatClient, NoOpChatClient>();

        services.AddHttpClient<EnvironmentDirectoryClient>(client =>
            {
                var options = configuration.GetSection("EnvironmentDirectory").Get<EnvironmentDirectoryOptions>();
                if (!string.IsNullOrWhiteSpace(options?.BaseUrl))
                {
                    client.BaseAddress = new Uri(options.BaseUrl);
                }
            })
            .AddStandardResilienceHandler();

        return services;
    }
}
