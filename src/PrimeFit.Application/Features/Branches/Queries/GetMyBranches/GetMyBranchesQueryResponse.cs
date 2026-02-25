using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetMyBranchesQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public BranchType BranchType { get; set; }
        public ImageDto Logo { get; set; } = null!;
        public BranchStatus BranchStatus { get; set; }
        public LocationDto Location { get; set; } = null!;
        public int SubscriptionsCount { get; set; }
    }

    public class LocationDto
    {
        public string Address { get; set; } = null!;
        public CoordinatesDto Coordinates { get; set; } = null!;
        public GovernorateDto Governorate { get; set; } = null!;
    }

    public class CoordinatesDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GovernorateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
    }
}

