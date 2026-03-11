using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetBranchPackagesQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesQueryResponse>>>, IAuthorizedRequest
    {

        public int BranchId { get; set; }


    }
}
