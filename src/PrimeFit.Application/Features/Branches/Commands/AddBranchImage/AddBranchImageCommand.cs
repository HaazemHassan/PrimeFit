using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Behaviors.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchImage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class AddBranchImageCommand : IRequest<ErrorOr<AddBranchImageCommandResponse>>, IAuthorizedRequest, ITransactionalRequest
    {

        public int BranchId { get; set; }
        public Stream ImageStream { get; set; } = default!;
        public BranchImageType ImageType { get; set; }


    }
}
