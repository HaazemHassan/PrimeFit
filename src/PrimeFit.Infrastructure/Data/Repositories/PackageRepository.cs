using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly DbSet<Package> _packages;

        public PackageRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _packages = context.Set<Package>();
        }
    }
}
