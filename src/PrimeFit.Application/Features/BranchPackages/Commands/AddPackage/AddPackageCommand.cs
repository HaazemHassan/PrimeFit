using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Packages.Commands.AddPackage
{

    [Authorize(Roles = [UserRole.Owner])]
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
