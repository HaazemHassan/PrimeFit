using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetBranchByIdQuery : IRequest<ErrorOr<GetBranchByIdQueryResponse>>, IAuthorizedRequest
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
