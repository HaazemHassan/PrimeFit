using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    [Authorize(UserTypes = [UserType.PartnerAdmin])]
    public class GetMyBranchesQuery
        : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetMyBranchesQueryResponse>>>
        , IAuthorizedRequest
    {

    }
}
