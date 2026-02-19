using PrimeFit.Domain.Common.Auditing;
using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities.Base
{
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity where TId : notnull
    {

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
