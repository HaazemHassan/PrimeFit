using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForUsers
{
    public class GetBranchPackagesForUsersQueryMappingProfile : Profile
    {
        public GetBranchPackagesForUsersQueryMappingProfile()
        {
            CreateMap<Package, GetBranchPackagesForUsersQueryResponse>();
        }
    }
}
