using Anshan.Domain;

namespace Ordering.Domain;

public class OrderPlaced : DomainEvent
{
    public OrderPlaced(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}