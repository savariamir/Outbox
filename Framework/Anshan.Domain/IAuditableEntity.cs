using System;

namespace Anshan.Domain
{
    public interface IAuditableEntity
    {
        public DateTime CreatedAt { get; }

        public DateTime ModifiedAt { get; }

        /// <summary>
        ///     Updates the <see cref="ModifiedAt" /> to the current time
        /// </summary>
        public void SetModifiedAt(DateTime dateTime);

        /// <summary>
        ///     Sets the <see cref="CreatedAt" /> to the current time
        /// </summary>
        public void SetCreatedAt(DateTime dateTime);
    }
}