using ErrorOr;

namespace PrimeFit.Api.Requests.Branches.GetBranchesForPublic;

public class GetBranchesForPublicRequest
{
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double RadiusInMeters { get; set; }
        public string? search { get; set; }
}
