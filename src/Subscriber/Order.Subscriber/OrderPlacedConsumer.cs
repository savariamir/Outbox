using MassTransit;
using Ordering.Domain;

namespace Order.Subscriber;

public class OrderPlacedConsumer: IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        throw new NotImplementedException();
    }
}