using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;

namespace PrimeFit.API.Requests.Branches
{
    public class UpdateWorkingHoursRequestMappingProfile : Profile
    {
        public UpdateWorkingHoursRequestMappingProfile()
        {
            CreateMap<UpdateWorkingHoursRequest, UpdateWorkingHoursCommand>();
        }
    }
}
