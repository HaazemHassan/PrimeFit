using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetUsersPaginatedQueryMappingProfile : Profile
    {
        public GetUsersPaginatedQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUsersPaginatedQueryResponse>()
           .ForMember(dest => dest.Phone,
              opt => opt.MapFrom(src => src.PhoneNumber));
        }

    }
}


