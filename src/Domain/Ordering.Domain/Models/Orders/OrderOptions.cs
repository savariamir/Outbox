namespace Ordering.Domain.Models.Orders;

public class OrderOptions
{
    public Guid Id { get; set; }
    public List<OrderItemOptions> OrderItemOptions { get; set; }
    public DateTime DeliveryDate { get; set; }
    public TimeSpan EarliestDeliveryTime { get; set; }
    public TimeSpan LatestDeliveryTime { get; set; }

    public string RecipientFullName { get;  set; }

    public string RecipientPhoneNumber { get;  set; }

    public string Line {  set; get; }

    public string PostalCode { get;  set; }
}