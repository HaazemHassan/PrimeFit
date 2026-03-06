using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{
    public class GetBranchByIdForPublicQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public BranchType BranchType { get; set; }
        public bool IsOpenNow { get; set; }
        public List<ImageDto> Images { get; set; } = null!;
        public LocationDto Location { get; set; } = null!;
        public double? DistanceInMeters { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
        public List<WorkingHoursDTO> WorkingHours { get; set; } = null!;
        public List<PackageDTO> Packages { get; set; } = null!;

    }

}
