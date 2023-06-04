using Anshan.Domain;
using MassTransit;

namespace Anshan.Messaging.IdempotentHandler;

public interface IMessageConsumer<in T> where T : DomainEvent
{
    Task Consume(ConsumeContext<T> context);
}