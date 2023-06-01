namespace Ordering.Domain.Models.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order);
}