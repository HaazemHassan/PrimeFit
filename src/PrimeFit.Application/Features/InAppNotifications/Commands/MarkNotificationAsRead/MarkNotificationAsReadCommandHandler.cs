using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.InAppNotifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler
        : IRequestHandler<MarkNotificationAsReadCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public MarkNotificationAsReadCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Success>> Handle(
            MarkNotificationAsReadCommand request,
            CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var notification = await _unitOfWork.UserNotifications
                .GetAsync(n => n.Id == request.NotificationId && n.UserId == userId, cancellationToken);

            if (notification is null)
            {
                return Error.NotFound(
                    description: "Notification not found.");
            }

            notification.MarkAsRead();

            await _unitOfWork.UserNotifications.UpdateAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
