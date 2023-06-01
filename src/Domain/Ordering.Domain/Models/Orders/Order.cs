using Anshan.Domain;
using Anshan.Messaging.Contracts;

namespace Ordering.Domain.Models.Orders;

public class Order : AggregateRoot<Guid>
{
    public Address Address { private set; get; }
    public IReadOnlyCollection<OrderItem> Items => _items;

    private readonly List<OrderItem> _items = new();
    public Shipping Shipping { get; private set; }
    public Price Price { get; private set; }

    private Order(OrderOptions options)
    {
        Id = options.Id;
        _items = options.OrderItemOptions
            .Select(p => OrderItem.CreateItem(p.ProductId, p.ProductName, p.Quantity, 1000, 100)).ToList();
        Shipping = new Shipping(options.DeliveryDate, options.EarliestDeliveryTime, options.LatestDeliveryTime);
        Price = Price.GetPrice(this);
        Address = new Address(options.Line, options.RecipientFullName, options.RecipientPhoneNumber, options.PostalCode);
        Publish(new OrderPlaced(options.Id));
    }

    /// <summary>
    /// For Storage
    /// </summary>
    private Order()
    {
    }

    public static Order PlaceOrder(OrderOptions options)
    {
        return new Order(options);
    }
}