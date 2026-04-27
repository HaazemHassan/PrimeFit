using PrimeFit.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours

{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchDetailsWrite])]
    public class UpdateWorkingHoursCommand : IRequest<ErrorOr<Success>>, IBranchAuthorizedRequest, ITransactionalRequest
    {
        public int BranchId { get; set; }
        public List<WorkingHourDto> WorkingHours { get; set; } = [];
    }

    public class WorkingHourDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
