using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class SubscriptionFreeze : AuditableEntity<int>
    {
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int MaxDays { get; set; }
        public int TotalDays => EndDate.HasValue ? (EndDate.Value - StartDate).Days : 0;

    }
}
