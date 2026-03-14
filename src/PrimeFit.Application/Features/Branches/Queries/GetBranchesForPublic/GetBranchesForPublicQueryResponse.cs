using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic

{
    public class GetBranchesForPublicQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public BranchType BranchType { get; set; }
        public string LogoUrl { get; set; } = null!;
        public string Governate { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? DistanceInMeters { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
    }
}


