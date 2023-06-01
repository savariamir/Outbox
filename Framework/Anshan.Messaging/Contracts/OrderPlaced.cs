using Anshan.Domain;

namespace Anshan.Messaging.Contracts;

public class OrderPlaced : DomainEvent
{
    public OrderPlaced(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}