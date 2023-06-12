using Anshan.Core;
using Anshan.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Anshan.Messaging.IdempotentHandler;

public class IdempotentMessageHandler<T> : IConsumer<T> where T : DomainEvent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDuplicateHandler _duplicateHandler;
    private readonly ILogger<IdempotentMessageHandler<T>> _logger;
    private readonly IMessageConsumer<T> _consumer;


    public IdempotentMessageHandler(IUnitOfWork unitOfWork,
        IDuplicateHandler duplicateHandler,
        ILogger<IdempotentMessageHandler<T>> logger, 
        IMessageConsumer<T> consumer)
    {
        _unitOfWork = unitOfWork;
        _duplicateHandler = duplicateHandler;
        _logger = logger;
        _consumer = consumer;
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        if (await _duplicateHandler.HasMessageConsumedBeforeAsync(context.Message.EventId))
        {
            _logger.LogInformation("{Type}-{MessageEventId} has been already consumed",
                context.Message.GetType(), context.Message.EventId);
            return;
        }

        try
        {
            await _unitOfWork.BeginAsync();
            
            if (_consumer is null)
            {
                _logger.LogError("Type of \'{Type}\' not found in event types", typeof(T));
                throw new ArgumentException($"There is no consumer for {typeof(T)}");
            }

            await _consumer.Consume(context);

            await _duplicateHandler.MarkMessageConsumed(context.Message.EventId);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception exception)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception(exception.Message, exception);
        }
    }
}