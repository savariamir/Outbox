using System.Collections.Generic;
using System.Threading.Tasks;
using Anshan.Persistence.Outbox;

namespace Anshan.OutboxProcessor.Repository
{
    public interface IOutboxRepository
    {
        Task<List<OutboxItem>> GetOutboxItemsAsync();

        Task UpdateOutboxItemsAsync(IEnumerable<OutboxItem> items);
    }
}