using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForOwner
{

    [Authorize(Roles = [UserRole.Owner])]
    public class GetBranchPackagesForOwnerQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesForOwnerQueryResponse>>>, IAuthorizedRequest
    {
        public GetBranchPackagesForOwnerQuery(int branchId)
        {
            BranchId = branchId;
        }
        public int BranchId { get; set; }


    }
}
