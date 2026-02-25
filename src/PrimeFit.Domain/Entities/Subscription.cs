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

        public SubscriptionStatus Status { get; private set; } = SubscriptionStatus.Scheduled;

        public DomainUser User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;
        public Package Package { get; private set; } = null!;

        private readonly List<SubscriptionFreeze> _freezes;
        public IReadOnlyCollection<SubscriptionFreeze> Freezes => _freezes.AsReadOnly();


        public bool IsExpired => GetStatus(DateTime.UtcNow) == SubscriptionStatus.Expired;
        public bool IsActive => GetStatus(DateTime.UtcNow) == SubscriptionStatus.Active;
        public bool IsCancelled => GetStatus(DateTime.UtcNow) == SubscriptionStatus.Cancelled;
        public bool IsScheduled => GetStatus(DateTime.UtcNow) == SubscriptionStatus.Scheduled;
        public bool IsFrozen => GetStatus(DateTime.UtcNow) == SubscriptionStatus.Frozen;



        public static ErrorOr<Subscription> Create(DomainUser user, Branch branch, Package package)
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

            subscription.SyncStatus(DateTime.UtcNow);
            return subscription;
        }


        public void SyncStatus(DateTime now)
        {
            Status = GetStatus(now);
        }

        public SubscriptionStatus GetStatus(DateTime now)
        {



            if (now >= GetEffectiveEndDate(now))
                return SubscriptionStatus.Expired;


            return SubscriptionStatus.Active;
        }


        public DateTime GetEffectiveEndDate(DateTime now)
        {
            if (EndDate is null)
                throw new InvalidOperationException("End date must be set to calculate effective end date.");

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




        public ErrorOr<Success> Activate(DateTime now)
        {
            if (Status != SubscriptionStatus.Scheduled)
            {
                return Error.Validation(description: "Only scheduled subscriptions can be activated.");
            }

            ActivationDate = now;
            EndDate = now.AddMonths(DurationInMonths);
            Status = SubscriptionStatus.Active;

            return Result.Success;

        }

        public ErrorOr<Success> Cancel(DateTime now)
        {
            if (Status == SubscriptionStatus.Cancelled)
            {
                return Error.Validation(description: "Subscription already cancelled");
            }

            if (Status == SubscriptionStatus.Expired)
            {
                return Error.Validation(description: "Can't cancel an expired subscription");
            }

            CancellationDate = now;
            Status = SubscriptionStatus.Cancelled;
            return Result.Success;

        }

    }
}
