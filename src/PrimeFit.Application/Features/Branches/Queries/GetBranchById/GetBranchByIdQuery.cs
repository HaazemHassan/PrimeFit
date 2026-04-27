using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchDetailsRead])]
    public class GetBranchByIdQuery : IRequest<ErrorOr<GetBranchByIdQueryResponse>>, IBranchAuthorizedRequest
    {

        public int BranchId { get; set; }

        public GetBranchByIdQuery()
        {
        }

        public GetBranchByIdQuery(int branchId)
        {
            BranchId = branchId;
        }


    }
}
