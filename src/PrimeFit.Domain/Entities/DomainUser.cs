using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public sealed class DomainUser : FullAuditableEntity<int>
    {
        public DomainUser(UserType userType,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            string? totpSecret = default)
        {
            _subscriptions = [];


            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            TotpSecret = totpSecret;
        }


        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public UserType UserType { get; private set; }
        public string? TotpSecret { get; private set; }
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

        public void UpdateEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
        }


    }
}
