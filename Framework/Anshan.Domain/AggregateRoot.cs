using System.Collections.Generic;

namespace Anshan.Domain
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregate, IVersionable
    {
        private List<DomainEvent> _domainEvents;

        public int Version { get; private set; } = 1;

        void IVersionable.IncrementVersion()
        {
            Version++;
        }

        protected AggregateRoot()
        {
            _domainEvents = new List<DomainEvent>();
        }

        public void Publish(DomainEvent domainEvent)
        {
            _domainEvents ??= new List<DomainEvent>();

            _domainEvents.Add(domainEvent);
        }

        public IReadOnlyCollection<DomainEvent> GetEvents()
        {
            return _domainEvents;
        }
    }
}