using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn
{

    [Authorize]
    public class CreateCheckInCommand : IRequest<ErrorOr<CreateCheckInCommandResponse>>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public int BranchId { get; set; }
    }
}
