using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetMyBranchesQueryMappingProfile : Profile
    {
        public GetMyBranchesQueryMappingProfile()
        {
            CreateMap<Branch, GetMyBranchesQueryResponse>();
            CreateMap<Governorate, GovernorateDto>();

        }

    }
}


