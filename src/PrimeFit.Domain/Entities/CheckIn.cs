using PrimeFit.Domain.Entities.Contracts;
using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities
{
    public class CheckIn : BaseEntity<int>, IHasCreationTime, IHasCreator, ISoftDeletableEntity
    {

        public int CustomerId { get; set; }
        public int BranchId { get; set; }
        public int SubscriptionId { get; set; }


        public int? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
