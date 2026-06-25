using MediatR;
using PrimeFit.Application.Common.Messaging;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.InAppNotifications.Events.SendRealTimeNotification;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.DomainEvents;

namespace PrimeFit.Application.Features.BranchPackages.Events
{
    public class BranchPackagesUpdatedDomainEventHandler : INotificationHandler<BranchPackagesUpdatedDomainEvent>
    {
        private readonly INotificationHelperService _notificationHelper;
        private readonly IInMemoryEventDispatcher _eventDispatcher;
        private readonly ICurrentUserService _currentUserService;

        public BranchPackagesUpdatedDomainEventHandler(
            INotificationHelperService notificationHelper,
            IInMemoryEventDispatcher eventDispatcher,
            ICurrentUserService currentUserService)
        {
            _notificationHelper = notificationHelper;
            _eventDispatcher = eventDispatcher;
            _currentUserService = currentUserService;
        }

        public async Task Handle(BranchPackagesUpdatedDomainEvent notificationEvent, CancellationToken cancellationToken)
        {
            int curUserId = _currentUserService.UserId!.Value;
            if (curUserId == notificationEvent.OwnerId)
            {
                return;

            }


            var notification = await _notificationHelper.AddNotificationAsync(
                userId: notificationEvent.OwnerId,
                title: "Branch Packages Updated",
                message: $"The packages of branch '{notificationEvent.BranchName}' have been updated by a staff member.",
                notificationType: NotificationType.BranchPackagesUpdate,
                ct: cancellationToken);

            _eventDispatcher.EnqueueEvent(new SendRealTimeNotificationEvent(notification));
        }
    }
}
