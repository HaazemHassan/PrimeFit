using PrimeFit.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Packages.Commands.UpdatePackage;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.PackagesWrite])]
    public class UpdatePackageCommand : IRequest<ErrorOr<UpdatePackageCommandResponse>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }
    }
}
