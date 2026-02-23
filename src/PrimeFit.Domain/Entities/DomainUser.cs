using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public sealed class DomainUser : FullAuditableEntity<int>
    {
        public DomainUser(string firstName, string lastName, string email, string phoneNumber)
        {
            _subscriptions = new();

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";




        private List<Subscription> _subscriptions { get; set; }
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();



        public void UpdateInfo(string? firstName = null, string? lastName = null, string? phoneNumber = null, string? address = null)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

        }


        public ErrorOr<Subscription> CreateSubscription(Branch branch, Package package, DateTime now)
        {
            var addSubscriptionResult = Subscription.Create(this, branch, package);

            if (addSubscriptionResult.IsError)
            {
                return addSubscriptionResult.Errors;

            }

            var subscription = addSubscriptionResult.Value;

            var hasActiveSubscription = _subscriptions.Any(s =>
                                      s.BranchId == branch.Id &&
                                      s.GetStatus(now) == SubscriptionStatus.Active);

            if (!hasActiveSubscription)
            {
                var activationResult = subscription.Activate(now);

                if (activationResult.IsError)
                {
                    return activationResult.Errors;

                }
            }

            _subscriptions.Add(subscription);
            return subscription;
        }



    }
}
