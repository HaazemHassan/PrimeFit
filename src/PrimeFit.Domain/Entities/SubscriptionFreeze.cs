using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class SubscriptionFreeze : AuditableEntity<int>
    {


        public SubscriptionFreeze(int subscriptionId, DateTimeOffset startDate, int maxDays)
        {
            SubscriptionId = subscriptionId;
            StartDate = startDate;
            MaxDays = maxDays;
        }

        private SubscriptionFreeze()
        {
        }

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int MaxDays { get; set; }
        public int TotalDays => EndDate.HasValue ? (int)Math.Ceiling((EndDate.Value - StartDate).TotalDays) : 0;

        public bool IsActive => !EndDate.HasValue;

    }
}
