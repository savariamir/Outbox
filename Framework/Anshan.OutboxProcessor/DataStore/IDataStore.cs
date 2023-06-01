using System.Threading.Tasks;

namespace Anshan.OutboxProcessor.DataStore
{
    public interface IDataStore
    {
        Task SubscribeForChanges();

        void SetSubscriber(IDataStoreChangeTracker outboxWorker);
    }
}