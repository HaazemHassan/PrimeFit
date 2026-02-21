using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdateLocationDetailsCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int GovernorateId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
