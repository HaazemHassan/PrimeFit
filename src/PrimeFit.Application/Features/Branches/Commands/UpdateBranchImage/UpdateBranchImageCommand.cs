using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdateBranchImageCommand : IAuthorizedRequest
    {
    }
}
