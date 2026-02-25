using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{

    [Authorize(Roles = [UserRole.Owner])]
    public class GetBranchByIdQuery : IRequest<ErrorOr<GetBranchByIdQueryResponse>>, IAuthorizedRequest
    {
        public GetBranchByIdQuery(int branchId)
        {
            BranchId = branchId;
        }

        public int BranchId { get; set; }

    }
}
