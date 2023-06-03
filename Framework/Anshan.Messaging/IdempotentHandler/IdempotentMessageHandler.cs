using Anshan.Core;
using Anshan.Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Anshan.Messaging.IdempotentHandler;

public class IdempotentMessageHandler<T> : IMessageConsumer<T> where T : DomainEvent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDuplicateHandler _duplicateHandler;
    private readonly IServiceProvider _serviceProvider;


    public IdempotentMessageHandler(IUnitOfWork unitOfWork, IDuplicateHandler duplicateHandler, IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _duplicateHandler = duplicateHandler;
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        await _unitOfWork.BeginAsync();

        try
        {
            if (!await _duplicateHandler.HasMessageProcessedBeforeAsync(context.Message.EventId))
            {
                var consumer = _serviceProvider.GetService<IMessageConsumer<T>>();

                if (consumer is null)
                    throw new ArgumentException($"There is no consumer for {typeof(T)}");
                
                await consumer.Consume(context);

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
}