using PrimeFit.Domain.Entities.Contracts;

namespace PrimeFit.Domain.Entities.Base
{

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity where TId : notnull
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
