using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Subscription : FullAuditableEntity<int>
    {
        private readonly List<SubscriptionFreeze> _freezes = new();
        public IReadOnlyCollection<SubscriptionFreeze> Freezes => _freezes.AsReadOnly();

        private Subscription() { }

        private Subscription(DomainUser user, Branch branch, Package package)
        {
            User = user;
            Branch = branch;
            Package = package;

            UserId = user.Id;
            BranchId = branch.Id;
            PackageId = package.Id;

            PaidAmount = package.Price;
            AllowedFreezeCount = package.NumberOfFreezes;
            AllowedFreezeDays = package.FreezeDurationInDays;
            DurationInMonths = package.DurationInMonths;
        }

        // =====================================================================
        // Properties
        // =====================================================================

        public int UserId { get; private set; }
        public int PackageId { get; private set; }
        public int BranchId { get; private set; }

        public DateTimeOffset? ActivationDate { get; private set; }
        public DateTimeOffset? EndDate { get; private set; }
        public DateTimeOffset? CancellationDate { get; private set; }
        public DateTimeOffset? NextProcessingDate { get; private set; }

        public decimal PaidAmount { get; private set; }
        public int AllowedFreezeCount { get; private set; }
        public int AllowedFreezeDays { get; private set; }
        public int DurationInMonths { get; private set; }

        public SubscriptionStatus Status { get; private set; } = SubscriptionStatus.Scheduled;

        public DomainUser User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;
        public Package Package { get; private set; } = null!;



        public static ErrorOr<Subscription> Create(DomainUser user, Branch branch, Package package)
        {
            if (package.BranchId != branch.Id)
                return Error.Validation(description: "This package doesn't belong to this branch.");

            if (branch.BranchStatus != BranchStatus.Active)
                return Error.Validation(description: "Cannot subscribe to an inactive branch.");

            if (!package.IsActive)
                return Error.Validation(description: "Cannot subscribe to an inactive package.");

            return new Subscription(user, branch, package);
        }



        /// <summary>
        /// Processes the subscription lifecycle.
        /// Returns true if the subscription just expired.
        /// </summary>
        public bool ProcessLifecycle(DateTimeOffset now)
        {
            if (Status != SubscriptionStatus.Active && Status != SubscriptionStatus.Frozen)
                return false;


            if (Status == SubscriptionStatus.Frozen)
            {
                var activeFreeze = _freezes.LastOrDefault(f => f.EndDate == null);

                if (activeFreeze is null)
                {
                    throw new InvalidOperationException("Subscription is frozen but no active freeze found.");
                }

                var freezeEnd = activeFreeze.StartDate.AddDays(activeFreeze.MaxDays);

                if (now < freezeEnd)
                {
                    return false;
                }

                UnFreeze(now);

                if (_freezes.Count < AllowedFreezeCount)
                {
                    Freeze(now);
                }

            }

            if (EndDate is null || now < EndDate)
                return false;

            Status = SubscriptionStatus.Expired;
            NextProcessingDate = null;
            return true;
        }



        private void RecalculateEndDate()
        {
            var totalFreezeDays = _freezes
                .Where(f => !f.IsActive)
                .Sum(f => f.TotalDays);

            var baseEnd = ActivationDate!.Value.AddMonths(DurationInMonths);
            EndDate = baseEnd.AddDays(totalFreezeDays);
        }



        public ErrorOr<Success> Activate(DateTimeOffset now)
        {
            if (Status != SubscriptionStatus.Scheduled)
                return Error.Validation(description: "Only scheduled subscriptions can be activated.");

            ActivationDate = now;
            EndDate = now.AddMonths(DurationInMonths);
            NextProcessingDate = EndDate;
            Status = SubscriptionStatus.Active;

            return Result.Success;
        }

        public ErrorOr<Success> Freeze(DateTimeOffset now)
        {

            if (Status != SubscriptionStatus.Active)
            {
                return Error.Validation(description: "Only active subscriptions can be frozen.");
            }


            if (_freezes.Count() >= AllowedFreezeCount)
            {
                return Error.Validation(description: "Freeze limit exceeded.");

            }

            var freeze = new SubscriptionFreeze(Id, now, AllowedFreezeDays);
            _freezes.Add(freeze);

            Status = SubscriptionStatus.Frozen;
            NextProcessingDate = now.AddDays(AllowedFreezeDays);

            return Result.Success;
        }


        public ErrorOr<Success> UnFreeze(DateTimeOffset now)
        {
            if (Status != SubscriptionStatus.Frozen)
            {
                return Error.Validation(description: "Only frozen subscriptions can be frozen.");
            }

            var activeFreeze = _freezes.LastOrDefault(f => f.EndDate == null);

            if (activeFreeze == null)
            {
                return Error.Validation(description: "No active freeze found.");
            }
            activeFreeze.EndDate = now;

            Status = SubscriptionStatus.Active;

            RecalculateEndDate();

            NextProcessingDate = EndDate;

            return Result.Success;
        }

        public ErrorOr<Success> Cancel(DateTimeOffset now)
        {
            if (Status == SubscriptionStatus.Cancelled)
                return Error.Validation(description: "Subscription is already cancelled.");

            if (Status == SubscriptionStatus.Expired)
                return Error.Validation(description: "Cannot cancel an expired subscription.");

            CancellationDate = now;
            NextProcessingDate = null;
            Status = SubscriptionStatus.Cancelled;

            return Result.Success;
        }

        // =====================================================================
        // Queries
        // =====================================================================

        public int GetRemainingDays(DateTimeOffset now)
        {
            if (!ActivationDate.HasValue)
                return DurationInMonths * 30;

            if (CancellationDate.HasValue || Status == SubscriptionStatus.Expired)
                return 0;

            var baseEndDate = ActivationDate.Value.AddDays(DurationInMonths * 30);

            var totalFreezeDuration = _freezes.Sum(f =>
            {
                var freezeEnd = f.EndDate ?? now;
                return (freezeEnd - f.StartDate).TotalDays;
            });

            var effectiveEndDate = baseEndDate.AddDays(totalFreezeDuration);

            if (effectiveEndDate <= now)
                return 0;

            return (int)Math.Ceiling((effectiveEndDate - now).TotalDays);
        }




        public void SetNextProcessingDate(DateTimeOffset? date)
        {
            NextProcessingDate = date;
        }


    }
}