using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public sealed class UserNotification : AuditableEntity<int>
    {
        public UserNotification(int userId, string title, string message, NotificationType notificationType)
        {
            UserId = userId;
            Title = title;
            Message = message;
            NotificationType = notificationType;
            IsRead = false;
        }

        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public bool IsRead { get; private set; }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
