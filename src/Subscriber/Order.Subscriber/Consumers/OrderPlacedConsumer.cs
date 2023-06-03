using Anshan.Messaging.Contracts;
using Anshan.Messaging.IdempotentHandler;
using MassTransit;

namespace Order.Subscriber.Consumers;

public class OrderPlacedConsumer : IMessageConsumer<OrderPlaced>
{
    private readonly ILogger<OrderPlacedConsumer> _logger;

    public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        _logger.LogInformation($"Order '{context.Message.EventId}- Consumed");
        return Task.CompletedTask;
    }
}
    

