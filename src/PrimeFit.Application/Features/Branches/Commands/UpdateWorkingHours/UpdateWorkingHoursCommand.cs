using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Behaviors.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.AddWorkingHours
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdateWorkingHoursCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest, ITransactionalRequest
    {
        public int BranchId { get; set; }
        public List<WorkingHourDto> WorkingHours { get; set; } = new();
    }

    public class WorkingHourDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
