using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anshan.Domain;
using Anshan.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Anshan.EF
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<OutboxItem> OutboxMessages { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            OnBeforeSaving();
            AddOutboxItemsToTransaction();
            return base.SaveChangesAsync(cancellationToken);
        }
        
        private void OnBeforeSaving()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.State is EntityState.Added or EntityState.Modified);

            var now = DateTime.UtcNow;
            foreach (var entry in entities)
            {
                if (!(entry.Entity is IAuditableEntity entity)) continue;

                if (entry.State == EntityState.Added) entity.SetCreatedAt(now);

                entity.SetModifiedAt(now);
            }
        }
        private void AddOutboxItemsToTransaction()
        {
            var outboxItems = ChangeTracker.Entries()
                .Select(a=>a.Entity)
                .OfType<IAggregate>()
                .SelectMany(a=> a.GetEvents())
                .Select(OutboxItemFactory.CreateOutboxItem)
                .ToList();
            
            OutboxMessages.AddRange(outboxItems);
        }
    }
}