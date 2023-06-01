using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anshan.OutboxProcessor.DataStore;
using Anshan.OutboxProcessor.Serialization;
using Anshan.OutboxProcessor.Types;
using Anshan.Persistence.Outbox;
using Coravel.Invocable;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Anshan.OutboxProcessor
{
    public class OutboxWorker : IInvocable
    {
        private readonly ILogger<OutboxWorker> _logger;
        private readonly IEventTypeResolver _typeResolver;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OutboxWorker(IEventTypeResolver typeResolver, ILogger<OutboxWorker> logger,
            IOutboxRepository outboxRepository, IPublishEndpoint publishEndpoint)
        {
            _typeResolver = typeResolver;
            _logger = logger;
            _outboxRepository = outboxRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Invoke()
        {
            await SubscribeForChanges();
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
                _publishEndpoint.Publish(eventToPublish);

                _logger.LogInformation($"Event '{item.EventType}-{item.EventId}' Published on bus.");
            }
        }

        private async Task SubscribeForChanges()
        {
            var items = await _outboxRepository.GetOutboxItemsAsync();
            if (items.Any())
            {
                _logger.LogInformation($"{items.Count} Events found in outbox");
                ChangeDetected(items);
                await _outboxRepository.UpdateOutboxItemsAsync(items);
            }
        }
    }
}