using PrimeFit.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.PackagesDelete])]
    public class DeletePackageCommand : IRequest<ErrorOr<Deleted>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
    }
}
