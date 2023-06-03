namespace Anshan.Messaging.IdempotentHandler;

public interface IDuplicateHandler
{
    Task<bool> HasMessageConsumedBeforeAsync(Guid messageId);
    Task MarkMessageConsumed(Guid messageId);
}