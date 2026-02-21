using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryMappingProfile : Profile
    {
        public GetUserByIdQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUserByIdQueryResponse>();
        }

    }
}