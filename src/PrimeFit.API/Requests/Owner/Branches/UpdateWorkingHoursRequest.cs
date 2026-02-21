using PrimeFit.Application.Features.Branches.Commands.AddWorkingHours;

namespace PrimeFit.API.Requests.Owner.Branches
{
    public class UpdateWorkingHoursRequest
    {
        public List<WorkingHourDto> WorkingHours { get; set; } = new();

    }
}
