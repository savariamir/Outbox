namespace Ordering.Domain;

public interface IOrderRepository
{
    Task AddAsync(Order order);
}