using ErrorOr;

namespace PrimeFit.Api.Requests.Branches.GetBranchesForPublic;

public class GetBranchesForPublicRequest
{
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double RadiusInMeters { get; set; }
        public string? search { get; set; }
}
