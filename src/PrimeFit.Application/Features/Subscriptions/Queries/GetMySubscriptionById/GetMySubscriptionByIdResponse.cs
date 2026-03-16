using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptionById
{
    public class GetMySubscriptionByIdResponse
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Governorate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public List<ImageDto> Images { get; set; } = new();



        public int SubscriptionId { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ActivationDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int DurationInDays { get; set; }
        public int CheckInsCount { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;

    }
}