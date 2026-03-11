using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class CreateCheckInCommand : IRequest<ErrorOr<CreateCheckInCommandResponse>>, IAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public int BranchId { get; set; }
    }
}
