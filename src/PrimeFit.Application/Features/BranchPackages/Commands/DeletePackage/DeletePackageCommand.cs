using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class DeletePackageCommand : IRequest<ErrorOr<Deleted>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
    }
}
