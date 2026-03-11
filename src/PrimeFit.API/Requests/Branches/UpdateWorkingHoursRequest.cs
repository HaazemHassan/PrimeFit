using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;

namespace PrimeFit.API.Requests.Branches
{
    public class UpdateWorkingHoursRequest
    {
        public List<WorkingHourDto> WorkingHours { get; set; } = new();

    }
}
