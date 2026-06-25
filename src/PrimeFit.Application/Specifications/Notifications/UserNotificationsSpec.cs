using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Specifications.Notifications
{
    public class UserNotificationsSpec : Specification<UserNotification>
    {
        public UserNotificationsSpec(int userId)
        {
            Query.Where(n => n.UserId == userId);
            Query.AsNoTracking();
        }
    }
}
