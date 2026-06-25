using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Notifications
{
    public class UserNotificationsPaginatedSpec : Specification<UserNotification>
    {
        public UserNotificationsPaginatedSpec(int userId, int pageNumber, int pageSize)
        {
            Query.Where(n => n.UserId == userId);
            Query.OrderByDescending(n => n.CreatedAt);
            Query.Paginate(pageNumber, pageSize);
            Query.AsNoTracking();
        }
    }
}
