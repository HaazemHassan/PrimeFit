using Ardalis.Specification;
using PrimeFit.Domain.Entities;


namespace PrimeFit.Application.Specifications.BranchReviews
{
    public class BranchReviewsPaginatedSpec : Specification<BranchReview>
    {
        public BranchReviewsPaginatedSpec(int branchId, int? rating, int? excludeUserId, int pageNumber, int pageSize)
        {

            if (rating.HasValue)
            {
                Query.Where(r => r.Rating == rating.Value);
            }


            if (excludeUserId.HasValue)
            {
                Query.Where(r => r.UserId != excludeUserId.Value);
            }


            Query.Where(r => r.BranchId == branchId)
                .OrderByDescending(r => r.CreatedAt)
                .Paginate(pageNumber, pageSize)
                .AsNoTracking();


        }
    }
}
