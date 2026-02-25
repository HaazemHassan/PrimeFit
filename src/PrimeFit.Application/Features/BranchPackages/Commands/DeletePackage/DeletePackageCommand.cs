using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Packages.Commands.DeletePackage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class DeletePackageCommand : IRequest<ErrorOr<Deleted>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int PackageId { get; set; }
    }
}
