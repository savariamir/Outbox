namespace Anshan.Messaging.IdempotentHandler;

public class DuplicationHandler : IDuplicateHandler
{
    public Task<bool> HasMessageConsumedBeforeAsync(Guid messageId)
    {
        return Task.FromResult(false);
    }

    public Task MarkMessageConsumed(Guid messageId)
    {
        return Task.CompletedTask;
    }
}