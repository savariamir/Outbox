using Anshan.Core;
using MassTransit;
using Order.Subscriber.IdempotentHandler;
using Ordering.Domain;

namespace Order.Subscriber.Consumers;

public class OrderPlacedConsumer : IdempotentMessageHandler<OrderPlaced>
{
    public OrderPlacedConsumer(IDuplicateHandler duplicateHandler, IUnitOfWork unitOfWork) : base(duplicateHandler,
        unitOfWork)
    {
    }

    protected override Task ConsumeAsync(ConsumeContext<OrderPlaced> context)
    {
        return Task.CompletedTask;
    }
}