using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {

        private readonly DbSet<Branch> _branches;


        public BranchRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _branches = context.Set<Branch>();
        }

    }
}
