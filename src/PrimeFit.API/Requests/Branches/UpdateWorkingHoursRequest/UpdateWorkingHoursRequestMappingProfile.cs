using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;

namespace PrimeFit.Api.Requests.Branches.UpdateWorkingHoursRequest {
    public class UpdateWorkingHoursRequestMappingProfile : Profile
    {
        public UpdateWorkingHoursRequestMappingProfile()
        {
            CreateMap<UpdateWorkingHoursRequest, UpdateWorkingHoursCommand>();
        }
    }
}
