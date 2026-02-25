using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Packages.Commands.UpdatePackage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdatePackageCommand : IRequest<ErrorOr<UpdatePackageCommandResponse>>, IAuthorizedRequest
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
