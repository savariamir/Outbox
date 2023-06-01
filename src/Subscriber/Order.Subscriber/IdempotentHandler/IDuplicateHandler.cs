namespace Order.Subscriber.IdempotentHandler;

public interface IDuplicateHandler
{
    Task<bool> HasMessageProcessedBeforeAsync(Guid messageId);
    Task MarkMessageProcessed(Guid messageId);
}