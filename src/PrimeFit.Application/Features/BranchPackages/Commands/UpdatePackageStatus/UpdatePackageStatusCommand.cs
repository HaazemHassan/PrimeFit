using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdatePackageStatusCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
        public bool IsActive { get; set; }

    }
}
