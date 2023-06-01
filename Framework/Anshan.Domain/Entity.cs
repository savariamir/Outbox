using System;

namespace Anshan.Domain
{
    public abstract class Entity<TKey> : IAuditableEntity, IDeletableEntity
    {
        public TKey Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime ModifiedAt { get; protected set; }
        private bool IsAuditDisabled { set; get; }

        /// <summary>
        ///     This method is hidden intentionally to avoid being accessed outside of entity
        /// </summary>
        public void EnableAudit() => IsAuditDisabled = false;

        public void DisableAudit() => IsAuditDisabled = true;

        public void SetModifiedAt(DateTime dateTime)
        {
            if (IsAuditDisabled) return;

            ModifiedAt = dateTime;
        }

        /// <summary>
        ///     This method is hidden intentionally to avoid being accessed outside of entity
        /// </summary>
        public void SetCreatedAt(DateTime dateTime)
        {
            if (IsAuditDisabled) return;

            CreatedAt = dateTime;
        }
        public bool IsDeleted { get; protected set; }

        public DateTime DeletedAt { get; protected set; }

        public void Delete()
        {
            IsDeleted = true;

            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = default;
            ModifiedAt = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType()) return false;
            var otherEntity = obj as Entity<TKey>;

            return Id.Equals(otherEntity.Id);
        }

        public void SetId(TKey id)
        {
            Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}