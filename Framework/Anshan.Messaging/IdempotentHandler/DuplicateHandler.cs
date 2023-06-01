namespace Anshan.Messaging.IdempotentHandler;

public class DuplicateHandler : IDuplicateHandler
{
    public Task<bool> HasMessageProcessedBeforeAsync(Guid messageId)
    {
        return Task.FromResult(false);
    }

    public Task MarkMessageProcessed(Guid messageId)
    {
        return Task.CompletedTask;
    }
}