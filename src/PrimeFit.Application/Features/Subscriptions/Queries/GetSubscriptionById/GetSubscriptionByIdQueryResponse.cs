using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionByIdQueryResponse
    {

        public int SubscriptionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ActivationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public int DurationInMonths { get; set; }
        public int RemainingDays { get; set; }
        public decimal PaidAmount { get; set; }
        public SubscriptionStatus Status { get; set; }
        public string PackageName { get; set; } = null!;
        public int TotalFreezesCount { get; set; }
        public int RemainingFreezesCount { get; set; }
        public MemberDTO Member { get; set; } = null!;

    }

    public class MemberDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

    }
}
