using ErrorOr;
using PrimeFit.Domain.DomainEvents;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Package : FullAuditableEntity<int>
    {

        public Package()
        {
            Subscriptions = new HashSet<Subscription>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }

        public int BranchId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
        public Branch Branch { get; set; } = null!;
        public string Currency { get; set; } = "egp";

        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; }
        public ErrorOr<Package> Update(string name, decimal price, int durationInMonths, int numberOfFreezes, int freezeDurationInDays)
        {
            Name = name;
            Price = price;
            DurationInMonths = durationInMonths;
            NumberOfFreezes = numberOfFreezes;
            FreezeDurationInDays = freezeDurationInDays;
            
            if (Branch != null)
            {
                RaiseDomainEvent(new BranchPackagesUpdatedDomainEvent(BranchId, Branch.OwnerId, Branch.Name));
            }
            return this;
        }


        public ErrorOr<Success> UpdateStatus(bool isActive)
        {
            IsActive = isActive;
            
            if (Branch != null)
            {
                RaiseDomainEvent(new BranchPackagesUpdatedDomainEvent(BranchId, Branch.OwnerId, Branch.Name));
            }
            return Result.Success;
        }

        public void Delete()
        {
            if (Branch != null)
            {
                RaiseDomainEvent(new BranchPackagesUpdatedDomainEvent(BranchId, Branch.OwnerId, Branch.Name));
            }
        }
    }
}
