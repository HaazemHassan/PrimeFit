using ErrorOr;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class BranchReview : AuditableEntity<int>
    {
        private BranchReview(int branchId, int userId, int rating, string? comment)
        {

            BranchId = branchId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
        }

        public int BranchId { get; private set; }
        public int UserId { get; private set; }


        public int Rating { get; private set; }

        public string? Comment { get; private set; }


        public DomainUser User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;



        public static ErrorOr<BranchReview> CreateBranchReview(int branchId, int userId, int rating, string? comment)
        {
            if (rating < 1 || rating > 5)
                return Error.Validation(description: "Rating must be between 1 and 5.");

            return new BranchReview(branchId, userId, rating, comment);

        }
    }
}
