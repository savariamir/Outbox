using System;

namespace Anshan.Domain
{
    public interface IDeletableEntity
    {
        public bool IsDeleted { get; }

        public DateTime DeletedAt { get; }

        void Delete();
    }
}