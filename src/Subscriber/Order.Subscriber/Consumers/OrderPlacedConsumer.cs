using Anshan.Core;
using Anshan.Messaging.Contracts;
using Anshan.Messaging.IdempotentHandler;
using MassTransit;

namespace Order.Subscriber.Consumers;

public class OrderPlacedConsumer : IdempotentMessageHandler<OrderPlaced>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(IDuplicateHandler duplicateHandler, IUnitOfWork unitOfWork,
        ILogger<OrderPlacedConsumer> logger) : base(duplicateHandler,
        unitOfWork)
    {
        _logger = logger;
    }

    protected override Task ConsumeAsync(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogInformation($"Order '{context.Message.EventId}- Consumed");
        return Task.CompletedTask;
    }
}