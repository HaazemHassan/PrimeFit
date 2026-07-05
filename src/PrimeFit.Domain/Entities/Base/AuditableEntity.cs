using PrimeFit.Domain.Entities.Contracts;

namespace PrimeFit.Domain.Entities.Base
{
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity where TId : notnull
    {

        public DateTimeOffset CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
