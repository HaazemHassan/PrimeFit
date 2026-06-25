using MediatR;
using PrimeFit.Application.Common.Messaging;
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

        public BranchPackagesUpdatedDomainEventHandler(
            INotificationHelperService notificationHelper,
            IInMemoryEventDispatcher eventDispatcher)
        {
            _notificationHelper = notificationHelper;
            _eventDispatcher = eventDispatcher;
        }

        public async Task Handle(BranchPackagesUpdatedDomainEvent notificationEvent, CancellationToken cancellationToken)
        {
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
