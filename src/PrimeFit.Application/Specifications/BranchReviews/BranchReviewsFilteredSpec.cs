using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.BranchReviews
{
    public class BranchReviewsFilteredSpec : Specification<BranchReview>
    {
        public BranchReviewsFilteredSpec(int branchId, int? rating, string? search = null)
        {
            Query.Where(r => r.BranchId == branchId);

            if (rating.HasValue)
            {
                Query.Where(r => r.Rating == rating.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                Query.Where(r =>
                    (r.Comment != null && r.Comment.ToLower().Contains(lowerSearch)) ||
                    r.User.FirstName.ToLower().Contains(lowerSearch) ||
                    r.User.LastName.ToLower().Contains(lowerSearch));
            }
        }
    }
}
