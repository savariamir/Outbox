using System.Collections.Generic;
using System.Threading.Tasks;
using Anshan.OutboxProcessor.DataStore;
using Anshan.OutboxProcessor.EventBus;
using Anshan.OutboxProcessor.Serialization;
using Anshan.OutboxProcessor.Types;
using Anshan.Persistence.Outbox;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace Anshan.OutboxProcessor
{
    public class OutboxWorker : IInvocable, IDataStoreChangeTracker
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OutboxWorker> _logger;
        private readonly IDataStore _store;
        private readonly IEventTypeResolver _typeResolver;

        public OutboxWorker(IEventTypeResolver typeResolver, ILogger<OutboxWorker> logger, IEventBus eventBus,
                            IDataStore store)
        {
            _typeResolver = typeResolver;
            _logger = logger;
            _eventBus = eventBus;
            _store = store;
            _store.SetSubscriber(this);
        }

        public void ChangeDetected(IEnumerable<OutboxItem> items)
        {
            foreach (var item in items)
            {
                var type = _typeResolver.GetType(item.EventType);
                
                if (type is null)
                {
                    _logger.LogError($"Type of '{item.EventType}' not found in event types");
                    continue;
                }

                var eventToPublish = EventDeserializer.Deserialize(type, item.EventBody);
                _eventBus.Publish(eventToPublish);
                _logger.LogInformation($"Event '{item.EventType}-{item.EventId}' Published on bus.");
            }
        }
        
        public async Task Invoke()
        {
            await _eventBus.Start();
            await _store.SubscribeForChanges();
        }
    }
}