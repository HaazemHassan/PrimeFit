using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class GovernorateRepository : GenericRepository<Governorate>, IGovernorateRepository
    {

        private readonly DbSet<Governorate> _governorates;


        public GovernorateRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _governorates = context.Set<Governorate>();
        }

    }
}
