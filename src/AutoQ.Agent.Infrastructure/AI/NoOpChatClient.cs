namespace AutoQ.Agent.Infrastructure.AI;

using Microsoft.Extensions.AI;

public sealed class NoOpChatClient : IChatClient
{
    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions options, CancellationToken cancellationToken)
    {
        var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, "{ \"status\": \"noop\" }"));
        return Task.FromResult(response);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions options,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await Task.Yield();
        yield return new ChatResponseUpdate(ChatRole.Assistant, "{ \"status\": \"noop\" }");
    }

    public object? GetService(Type serviceType, object? serviceKey) =>
        serviceType.IsAssignableFrom(GetType()) ? this : null;
}
