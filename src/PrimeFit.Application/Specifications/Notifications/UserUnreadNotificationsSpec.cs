using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Notifications
{
    public class UserUnreadNotificationsSpec : Specification<UserNotification>
    {
        public UserUnreadNotificationsSpec(int userId)
        {
            Query.Where(n => n.UserId == userId && !n.IsRead);
            Query.AsNoTracking();
        }
    }
}
