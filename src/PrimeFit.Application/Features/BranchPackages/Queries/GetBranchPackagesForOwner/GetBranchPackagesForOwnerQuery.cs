using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForOwner
{

    [Authorize(Roles = [UserRole.Owner])]
    public class GetBranchPackagesForOwnerQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesForOwnerQueryResponse>>>, IAuthorizedRequest
    {

        public int BranchId { get; set; }


    }
}
