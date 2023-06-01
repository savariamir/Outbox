using System;
using Anshan.Domain;

namespace Anshan.Persistence.Outbox
{
    public class OutboxItem : Entity<string>
    {
        public OutboxItem()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EventId { get; set; }

        public string EventType { get; set; }

        public string EventBody { get; set; }

        public DateTime? PublishDateTime { get; set; }
    }
}