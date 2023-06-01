using Ordering.Domain;

namespace Ordering.Persistence.EF;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingDBContext _dbContext;

    public OrderRepository(OrderingDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
    {
        _dbContext.Add(order);
        await _dbContext.SaveChangesAsync();
    }
}