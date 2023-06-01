using System.Collections.Generic;
using Anshan.Persistence.Outbox;

namespace Anshan.OutboxProcessor.DataStore
{
    public interface IDataStoreChangeTracker
    {
        void ChangeDetected(IEnumerable<OutboxItem> item);
    }
}