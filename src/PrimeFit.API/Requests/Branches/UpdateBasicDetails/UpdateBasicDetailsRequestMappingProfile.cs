using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails;

namespace PrimeFit.Api.Requests.Branches.UpdateBasicDetails {
    public class UpdateBasicDetailsRequestMappingProfile : Profile
    {
        public UpdateBasicDetailsRequestMappingProfile()
        {
            CreateMap<UpdateBasicDetailsRequest, UpdateBasicDetailsCommand>();
        }
    }
}
