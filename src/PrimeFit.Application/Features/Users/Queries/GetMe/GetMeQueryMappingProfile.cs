using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetMe
{
    internal class GetMeQueryMappingProfile : Profile
    {
        public GetMeQueryMappingProfile()
        {
            CreateMap<Branch, BranchLiteDto>();


        }
    }
}
