using System;

namespace Anshan.Domain
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            EventId = Guid.NewGuid();
        }

        public virtual Type AggregateRootType { get; }

        public Guid CorrelationId { get; private set; }

        public Guid EventId { get; }

        public Guid CausationId { get; private set; }

        public long Timestamp { get; }

        public void SetCorrelationId(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public void SetCausationId(Guid causationId)
        {
            CausationId = causationId;
        }
    }
}