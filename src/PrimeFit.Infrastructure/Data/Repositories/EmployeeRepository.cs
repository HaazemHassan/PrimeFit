using AutoMapper;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
