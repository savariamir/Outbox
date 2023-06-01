namespace Ordering.Domain.Models.Orders;

public class OrderItemOptions
{
    public int ProductId {  set; get; }

    public string ProductName {  set; get; }

    public int Quantity {  set; get; }

    public decimal UnitPrice {  set; get; }

    public decimal DiscountAmount {  set; get; }

    public decimal Discount {  set; get; }
}