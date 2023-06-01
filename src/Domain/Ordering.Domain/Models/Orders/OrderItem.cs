using Anshan.Domain;

namespace Ordering.Domain.Models.Orders;

public class OrderItem : ValueObject
{
    public int ProductId { private set; get; }

    public string ProductName { private set; get; }

    public int Quantity { private set; get; }

    public decimal UnitPrice { private set; get; }

    public decimal DiscountAmount { private set; get; }

    public decimal Discount { private set; get; }

    public OrderItem(int productId, string productName, int quantity,
        decimal unitPrice, decimal discount, decimal discountAmount)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        Discount = discount;
    }

    public static OrderItem CreateItem(int productId, string productName, int quantity, decimal unitPrice,
        decimal discount)
    {
        var discountAmount = (int)(unitPrice * quantity * (discount / 100));
        return new OrderItem(productId, productName, quantity, unitPrice, discount,
            discountAmount);
    }
}