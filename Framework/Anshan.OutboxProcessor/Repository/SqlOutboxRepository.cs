using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anshan.EF;
using Anshan.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Anshan.OutboxProcessor.Repository
{
    public class SqlOutboxRepository : IOutboxRepository
    {
        private readonly CoreDbContext _context;

        public SqlOutboxRepository(CoreDbContext context)
        {
            _context = context;
        }

        public async Task UpdateOutboxItemsAsync(IEnumerable<OutboxItem> items)
        {
            foreach (var item in items)
            {
                item.PublishDateTime = DateTime.UtcNow;
                _context.Update(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<OutboxItem>> GetOutboxItemsAsync()
        {
            return await _context.OutboxMessages.Where(p => p.PublishDateTime == null).Take(100).ToListAsync();
        }
    }
}