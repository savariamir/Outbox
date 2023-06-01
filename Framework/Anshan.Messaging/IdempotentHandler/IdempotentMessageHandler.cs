using Anshan.Core;
using Anshan.Domain;
using MassTransit;

namespace Anshan.Messaging.IdempotentHandler;

public abstract class IdempotentMessageHandler<T> : IConsumer<T> where T : DomainEvent
{
    private readonly IDuplicateHandler _duplicateHandler;
    private readonly IUnitOfWork _unitOfWork;

    protected IdempotentMessageHandler(IDuplicateHandler duplicateHandler, IUnitOfWork unitOfWork)
    {
        _duplicateHandler = duplicateHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        await _unitOfWork.BeginAsync();

        try
        {
            if (!await _duplicateHandler.HasMessageProcessedBeforeAsync(context.Message.EventId))
            {
                await ConsumeAsync(context);
                await _duplicateHandler.MarkMessageProcessed(context.Message.EventId);
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception exception)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception(exception.Message, exception);
        }
    }

    protected abstract Task ConsumeAsync(ConsumeContext<T> context);
}
