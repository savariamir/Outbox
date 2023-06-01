using Anshan.EF;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models.Orders;

namespace Ordering.Persistence.EF;

public class OrderingDBContext : CoreDbContext
{
    public OrderingDBContext(DbContextOptions<OrderingDBContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityConfiguration).Assembly);
    }
}