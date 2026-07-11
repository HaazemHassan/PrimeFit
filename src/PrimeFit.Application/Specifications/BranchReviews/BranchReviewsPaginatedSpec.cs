using Ardalis.Specification;
using PrimeFit.Domain.Entities;


namespace PrimeFit.Application.Specifications.BranchReviews
{
    public class BranchReviewsPaginatedSpec : BranchReviewsFilteredSpec
    {
        public BranchReviewsPaginatedSpec(int branchId, int? rating, int? excludeUserId, int pageNumber, int pageSize, string? search = null)
            : base(branchId, rating, search)
        {
            if (excludeUserId.HasValue)
            {
                Query.Where(r => r.UserId != excludeUserId.Value);
            }

            Query.OrderByDescending(r => r.CreatedAt)
                .Paginate(pageNumber, pageSize)
                .AsNoTracking();

        }
    }
}
