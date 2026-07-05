using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;

namespace PrimeFit.Api.Requests.Branches.UpdateWorkingHoursRequest {
    public class UpdateWorkingHoursRequest
    {
        public List<WorkingHourDto> WorkingHours { get; set; } = new();

    }
}
