using Anshan.Domain;
using MassTransit;

namespace Anshan.Messaging.IdempotentHandler;

public interface IMessageConsumer<in T> : IConsumer<T> where T : DomainEvent
{
}