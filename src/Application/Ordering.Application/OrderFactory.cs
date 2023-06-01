using Ordering.Application.Contracts;
using Ordering.Domain;

namespace Ordering.Application;

public static class OrderFactory
{
    public static OrderOptions CreateFrom(PlaceOrderCommand command)
    {
        var options = new OrderOptions
        {
            Id = Guid.NewGuid(),
            DeliveryDate = command.DeliveryDate,
            EarliestDeliveryTime = command.EarliestDeliveryTime,
            Line = command.Line,
            PostalCode = command.PostalCode,
            RecipientPhoneNumber = command.RecipientPhoneNumber,
            RecipientFullName = command.RecipientFullName,
            OrderItemOptions = command.OrderItems.Select(p => new OrderItemOptions
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Quantity = p.Quantity,
                UnitPrice = 1_000M,
                DiscountAmount = 100,
                Discount = 10
            }).ToList()
        };

        return options;
    }
}