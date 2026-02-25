using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdateBussinessDetailsCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public BranchType? BranchType { get; set; }

    }
}
