using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class BranchImageRepostiory : GenericRepository<BranchImage>, IBranchImageRepository
    {

        private readonly DbSet<BranchImage> _brancheImages;


        public BranchImageRepostiory(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _brancheImages = context.Set<BranchImage>();
        }

    }
}
