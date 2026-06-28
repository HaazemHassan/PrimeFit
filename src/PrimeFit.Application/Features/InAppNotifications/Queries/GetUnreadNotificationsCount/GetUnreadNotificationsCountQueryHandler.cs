using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Notifications;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetUnreadNotificationsCount
{
    public class GetUnreadNotificationsCountQueryHandler
        : IRequestHandler<GetUnreadNotificationsCountQuery, ErrorOr<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetUnreadNotificationsCountQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<int>> Handle(
            GetUnreadNotificationsCountQuery request,
            CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var unreadSpec = new UserUnreadNotificationsSpec(userId);
            var unreadCount = await _unitOfWork.UserNotifications
                .CountAsync(unreadSpec, cancellationToken);

            return unreadCount;
        }
    }
}
