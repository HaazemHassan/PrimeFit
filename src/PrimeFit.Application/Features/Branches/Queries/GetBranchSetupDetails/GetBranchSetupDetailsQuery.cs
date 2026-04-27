using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchDetailsRead])]
    public class GetBranchSetupDetailsQuery : IRequest<ErrorOr<GetBranchSetupDetailsQueryResponse>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }

        public GetBranchSetupDetailsQuery()
        {
        }

        public GetBranchSetupDetailsQuery(int branchId)
        {
            BranchId = branchId;
        }
    }
}
