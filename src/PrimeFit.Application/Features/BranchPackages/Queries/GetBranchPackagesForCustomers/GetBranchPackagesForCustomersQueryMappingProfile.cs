using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQueryMappingProfile : Profile
    {
        public GetBranchPackagesForCustomersQueryMappingProfile()
        {
            CreateMap<Package, GetBranchPackagesForCustomersQueryResponse>();
        }
    }
}
