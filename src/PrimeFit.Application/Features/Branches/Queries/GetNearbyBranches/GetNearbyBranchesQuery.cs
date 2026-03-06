using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.Branches.Queries.GetNearbyBranches
{
    public class GetNearbyBranchesQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetNearbyBranchesQueryResponse>>>
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusInMeters { get; set; } = 10000;
        public string? search { get; set; } = string.Empty;

    }
}
