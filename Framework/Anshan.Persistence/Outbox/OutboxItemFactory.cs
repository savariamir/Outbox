using System;
using Anshan.Domain;
using Newtonsoft.Json;

namespace Anshan.Persistence.Outbox
{
    public static class OutboxItemFactory
    {
        public static OutboxItem CreateOutboxItem(IDomainEvent @event)
        {
            return new()
            {
                EventId = @event.EventId,
                EventType = @event.GetType().Name,
                PublishDateTime = null,
                EventBody = JsonConvert.SerializeObject(@event)
            };
        }
    }
}