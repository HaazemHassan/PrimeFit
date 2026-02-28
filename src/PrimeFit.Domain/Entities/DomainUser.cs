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




        private readonly List<Subscription> _subscriptions;
        public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();



        public void UpdateInfo(string? firstName = null, string? lastName = null, string? phoneNumber = null)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName;

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;


        }


    }
}
