using System;

namespace Anshan.Domain
{
    // Chain
    // Command -> Event -> Command

    public interface IDomainEvent
    {
        /// <summary>
        ///     Represents the aggregate that owns the event
        /// </summary>
        public Type AggregateRootType { get; }

        /// <summary>
        ///     Pointer to the first command/event id in a chain of events
        /// </summary>
        /// <remarks>
        ///     It's used to identify long-running transactions.
        /// </remarks>
        Guid CorrelationId { get; }

        /// <summary>
        ///     Identifier of the event
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        ///     Pointer to the predecessor of the event. In simple terms, this identifier refers to the command that caused this
        ///     event
        /// </summary>
        Guid CausationId { get; }

        /// <summary>
        ///     The timestamp of event in the unix format
        /// </summary>
        long Timestamp { get; }
    }
}