using Ardalis.Specification;
using PrimeFit.Application.Specifications;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions
{
    public class UserSubscriptionsSpec : Specification<Subscription>
    {
        public UserSubscriptionsSpec(int userId, SubscriptionStatus? status, int pageNumber, int pageSize)
        {
            Query.Where(s => s.UserId == userId);

            if (status.HasValue)
            {
                Query.Where(s => s.Status == status.Value);
            }

            Query.Include(s => s.Branch);
            Query.Include(s => s.Branch.Images.Where(bi => bi.Type == BranchImageType.Logo));
            Query.Include(s => s.Package);
            Query.Include(s => s.Freezes);

            Query.OrderByDescending(s => s.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }

}
