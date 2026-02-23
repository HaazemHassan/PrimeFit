using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Package : FullAuditableEntity<int>
    {

        public Package()
        {
            Subscriptions = new HashSet<Subscription>();

        }

        public int BranchId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
        public Branch Branch { get; set; } = null!;

        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
