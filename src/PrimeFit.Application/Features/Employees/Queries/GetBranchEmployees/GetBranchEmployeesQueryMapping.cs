using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    public class GetBranchEmployeesQueryMapping : Profile
    {
        public GetBranchEmployeesQueryMapping()
        {
            CreateMap<Employee, GetBranchEmployeesQueryResponse>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
               .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Active));


        }

    }
}
