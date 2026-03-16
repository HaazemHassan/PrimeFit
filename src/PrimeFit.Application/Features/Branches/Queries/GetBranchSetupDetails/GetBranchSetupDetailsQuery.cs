using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetBranchSetupDetailsQuery : IRequest<ErrorOr<GetBranchSetupDetailsQueryResponse>>, IAuthorizedRequest
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
