using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class BranchReviewRepository : GenericRepository<BranchReview>, IBranchReviewRepository
    {

        private readonly DbSet<BranchReview> _branchReviews;


        public BranchReviewRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _branchReviews = context.Set<BranchReview>();
        }

    }
}
