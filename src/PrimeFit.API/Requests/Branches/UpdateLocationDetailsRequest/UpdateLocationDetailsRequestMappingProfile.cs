using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;

namespace PrimeFit.Api.Requests.Branches.UpdateLocationDetailsRequest {
    public class UpdateLocationDetailsRequestMappingProfile : Profile
    {
        public UpdateLocationDetailsRequestMappingProfile()
        {
            CreateMap<UpdateLocationDetailsRequest, UpdateLocationDetailsCommand>();
        }
    }
}
