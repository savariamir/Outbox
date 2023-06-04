using Anshan.Core;
using Anshan.Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Anshan.Messaging.IdempotentHandler;

public class IdempotentMessageHandler<T> : IConsumer<T> where T : DomainEvent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDuplicateHandler _duplicateHandler;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<IdempotentMessageHandler<T>> _logger;


    public IdempotentMessageHandler(IUnitOfWork unitOfWork, IDuplicateHandler duplicateHandler,
        IServiceProvider serviceProvider, ILogger<IdempotentMessageHandler<T>> logger)
    {
        _unitOfWork = unitOfWork;
        _duplicateHandler = duplicateHandler;
        _serviceProvider = serviceProvider;
        _logger = logger;
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

            var consumer = _serviceProvider.GetService<IMessageConsumer<T>>();
            if (consumer is null)
            {
                _logger.LogError("Type of \'{Type}\' not found in event types", typeof(T));
                throw new ArgumentException($"There is no consumer for {typeof(T)}");
            }

            await consumer.Consume(context);

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