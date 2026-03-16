using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails;

namespace PrimeFit.API.Requests.Branches
{
    public class UpdateBussinessDetailsRequestMappingProfile : Profile
    {
        public UpdateBussinessDetailsRequestMappingProfile()
        {
            CreateMap<UpdateBussinessDetailsRequest, UpdateBussinessDetailsCommand>();
        }
    }
}
