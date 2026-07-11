using Ardalis.Specification;
using PrimeFit.Domain.Entities;


namespace PrimeFit.Application.Specifications.BranchReviews
{
    public class BranchReviewsPaginatedSpec : Specification<BranchReview>
    {
        public BranchReviewsPaginatedSpec(int branchId, int? rating, int? excludeUserId, int pageNumber, int pageSize, string? search = null)
        {

            if (rating.HasValue)
            {
                Query.Where(r => r.Rating == rating.Value);
            }


            if (excludeUserId.HasValue)
            {
                Query.Where(r => r.UserId != excludeUserId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Include(r => r.User).Where(r => 
                    (r.Comment != null && r.Comment.ToLower().Contains(lowerSearch)) || 
                    r.User.FirstName.ToLower().Contains(lowerSearch) || 
                    r.User.LastName.ToLower().Contains(lowerSearch));
            }


            Query.Where(r => r.BranchId == branchId)
                .OrderByDescending(r => r.CreatedAt)
                .Paginate(pageNumber, pageSize)
                .AsNoTracking();

        }
    }
}
