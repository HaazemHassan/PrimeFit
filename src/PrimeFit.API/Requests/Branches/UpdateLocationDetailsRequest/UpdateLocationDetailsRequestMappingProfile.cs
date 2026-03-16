using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;

namespace PrimeFit.API.Requests.Branches
{
    public class UpdateLocationDetailsRequestMappingProfile : Profile
    {
        public UpdateLocationDetailsRequestMappingProfile()
        {
            CreateMap<UpdateLocationDetailsRequest, UpdateLocationDetailsCommand>();
        }
    }
}
