using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Packages.Commands.AddPackage;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.BranchPackages.Commands.AddPackage
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class AddPackageCommand : IRequest<ErrorOr<AddPackageCommandResponse>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; } = true;
        public int NumberOfFreezes { get; set; }
        public int FreezeDurationInDays { get; set; }

    }
}
