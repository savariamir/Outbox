namespace Ordering.Application.Contracts.Orders;

public class OrderItemCommand
{
    public int ProductId {  set; get; }

    public string ProductName {  set; get; }

    public int Quantity {  set; get; }
}