using Anshan.Domain;

namespace Ordering.Domain.Models.Orders;

public class Shipping : ValueObject
{
    public Shipping(DateTime deliveryDate, TimeSpan earliestDeliveryTime, TimeSpan latestDeliveryTime)
    {
        Cost = 100;
        DeliveryDate = deliveryDate;
        EarliestDeliveryTime = earliestDeliveryTime;
        LatestDeliveryTime = latestDeliveryTime;
    }

    public decimal Cost { get; private set; }
    public DateTime DeliveryDate { get; private set; }
    public TimeSpan LatestDeliveryTime { get; private set; }
    public TimeSpan EarliestDeliveryTime { get; private set; }
}