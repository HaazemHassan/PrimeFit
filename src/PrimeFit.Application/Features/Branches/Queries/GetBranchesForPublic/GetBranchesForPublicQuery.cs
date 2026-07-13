using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic
{
    public class GetBranchesForPublicQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchesForPublicQueryResponse>>>
    {

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double RadiusInMeters { get; set; } = 100000;
        public string? search { get; set; } = string.Empty;

    }
}
