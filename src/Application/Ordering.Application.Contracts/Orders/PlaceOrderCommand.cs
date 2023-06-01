using LiteBus.Commands.Abstractions;

namespace Ordering.Application.Contracts.Orders;

public class PlaceOrderCommand : ICommand
{
    public DateTime DeliveryDate { get; set; }
    public TimeSpan EarliestDeliveryTime { get; set; }
    
    public string RecipientFullName { get; set; }

    public string RecipientPhoneNumber { get; set; }

    public string Line { set; get; }

    public string PostalCode { get; set; }
    public IEnumerable<OrderItemCommand> OrderItems { get; set; }
}