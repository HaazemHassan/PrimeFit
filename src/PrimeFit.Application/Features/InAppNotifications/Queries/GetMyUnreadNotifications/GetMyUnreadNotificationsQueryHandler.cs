using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyUnreadNotifications
{
    public class GetMyUnreadNotificationsQueryHandler
        : IRequestHandler<GetMyUnreadNotificationsQuery, ErrorOr<List<InAppNotificationDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetMyUnreadNotificationsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<List<InAppNotificationDto>>> Handle(
            GetMyUnreadNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var notifications = await _unitOfWork.UserNotifications
                .ListAsync<InAppNotificationDto>(
                    n => n.UserId == userId && !n.IsRead,
                    cancellationToken);

            return notifications;
        }
    }
}
