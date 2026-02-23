using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Subscription : FullAuditableEntity<int>
    {

        private Subscription(DomainUser user, Branch branch, Package package)
        {
            _freezes = new();


            User = user;
            Branch = branch;
            Package = package;

            UserId = user.Id;
            BranchId = branch.Id;
            PackageId = package.Id;

            PaidAmount = package.Price;

        }

        private Subscription()
        {
            _freezes = new();

        }

        public int UserId { get; private set; }
        public int PackageId { get; private set; }
        public int BranchId { get; private set; }
        public DateTime? ActivationDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public decimal PaidAmount { get; private set; }
        public int AllowedFreezeCount { get; private set; }
        public int AllowedFreezeDays { get; private set; }
        public int DurationInMonths { get; private set; }

        public DomainUser User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;
        public Package Package { get; private set; } = null!;

        private List<SubscriptionFreeze> _freezes;
        public IReadOnlyCollection<SubscriptionFreeze> Freezes => _freezes.AsReadOnly();



        internal static ErrorOr<Subscription> Create(DomainUser user, Branch branch, Package package)
        {
            if (branch.BranchStatus != BranchStatus.Active)
            {
                return Error.Validation(description: "Cannot subscribe to an inactive branch.");

            }
            if (package.BranchId != branch.Id)
            {
                return Error.Validation(description: "This package doesn't belong to this branch");

            }
            if (!package.IsActive)
            {
                return Error.Validation(description: "Cannot subscribe to an inactive package.");
            }

            var subscription = new Subscription(user, branch, package)
            {

                AllowedFreezeCount = package.NumberOfFreezes,
                AllowedFreezeDays = package.FreezeDurationInDays,
                DurationInMonths = package.DurationInMonths,

            };
            return subscription;
        }


        public SubscriptionStatus GetStatus(DateTime now)
        {

            if (CancellationDate.HasValue)
                return SubscriptionStatus.Cancelled;

            if (!ActivationDate.HasValue)
                return SubscriptionStatus.Scheduled;

            if (now < ActivationDate.Value)
                return SubscriptionStatus.Scheduled;

            if (IsCurrentlyFrozen(now))
                return SubscriptionStatus.Frozen;

            if (now >= GetEffectiveEndDate(now))
                return SubscriptionStatus.Expired;


            return SubscriptionStatus.Active;
        }


        public DateTime GetEffectiveEndDate(DateTime now)
        {

            var totalFreezeDays = _freezes.Sum(f =>
            {
                var freezeEnd = f.EndDate ?? now;
                return (freezeEnd - f.StartDate).Days;
            });

            return EndDate.Value.AddDays(totalFreezeDays);
        }


        public int GetRemainingDays(DateTime now)
        {
            if (!ActivationDate.HasValue || !EndDate.HasValue)
                return DurationInMonths * 30;


            if (CancellationDate.HasValue)
                return 0;

            var effectiveEnd = GetEffectiveEndDate(now);

            if (now >= effectiveEnd)
                return 0;

            return (int)Math.Ceiling((effectiveEnd - now).TotalDays);
        }

        public bool IsCurrentlyFrozen(DateTime now)
        {
            return _freezes.Any(f => now >= f.StartDate && (!f.EndDate.HasValue || now <= f.EndDate));
        }


        public ErrorOr<Success> Activate(DateTime now)
        {
            var status = GetStatus(now);
            if (status != SubscriptionStatus.Scheduled)
            {
                return Error.Validation(description: "Only scheduled subscriptions can be activated.");
            }

            ActivationDate = now;
            EndDate = now.AddMonths(DurationInMonths);

            return Result.Success;

        }

        public ErrorOr<Success> Cancel(DateTime now)
        {
            var status = GetStatus(now);
            if (status == SubscriptionStatus.Cancelled)
            {
                return Error.Validation(description: "Subscription already cancelled");
            }

            if (status == SubscriptionStatus.Expired)
            {
                return Error.Validation(description: "Can't cancel an expired subscription");
            }

            CancellationDate = now;

            return Result.Success;

        }

    }
}
