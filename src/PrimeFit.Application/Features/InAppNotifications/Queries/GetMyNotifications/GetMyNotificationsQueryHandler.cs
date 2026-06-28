using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Notifications;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQueryHandler
        : IRequestHandler<GetMyNotificationsQuery, ErrorOr<PaginatedResult<InAppNotificationDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetMyNotificationsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<InAppNotificationDto>>> Handle(
            GetMyNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var dataSpec = new UserNotificationsPaginatedSpec(userId, request.PageNumber, request.PageSize);
            var countSpec = new UserNotificationsSpec(userId);

            var notifications = await _unitOfWork.UserNotifications
                .ListAsync<InAppNotificationDto>(dataSpec, cancellationToken);

            var totalCount = await _unitOfWork.UserNotifications
                .CountAsync(countSpec, cancellationToken);

            // Get unread count
            var unreadSpec = new UserUnreadNotificationsSpec(userId);
            var unreadCount = await _unitOfWork.UserNotifications
                .CountAsync(unreadSpec, cancellationToken);

            var result = new PaginatedResult<InAppNotificationDto>(notifications, totalCount, request.PageNumber, request.PageSize);
            result.Meta = new NotificationsMeta
            {
                UnreadCount = unreadCount
            };

            return result;
        }
    }
}
