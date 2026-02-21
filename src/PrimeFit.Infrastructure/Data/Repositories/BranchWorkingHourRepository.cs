using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class BranchWorkingHourRepository : GenericRepository<BranchWorkingHour>, IBranchWorkingHourRepository
    {

        private readonly DbSet<BranchWorkingHour> _branchWorkingHours;


        public BranchWorkingHourRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _branchWorkingHours = context.Set<BranchWorkingHour>();
        }

    }
}
