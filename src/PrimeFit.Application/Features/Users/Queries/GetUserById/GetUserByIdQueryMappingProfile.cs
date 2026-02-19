using AutoMapper;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryMappingProfile : Profile
    {
        public GetUserByIdQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUserByIdQueryResponse>()
                           .IncludeBase<DomainUser, UserResponse>();
        }

    }
}