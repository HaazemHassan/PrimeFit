using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Events
{
    internal sealed class SubscriptionCreatedIntegrationEventHandler
        : INotificationHandler<SubscriptionCreatedIntegrationEvent>
    {
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionCreatedIntegrationEventHandler(
            IPushNotificationService pushNotificationService,
            IUnitOfWork unitOfWork)
        {
            _pushNotificationService = pushNotificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SubscriptionCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var tokensSpec = new TokensByUserIdSpec(notification.UserId);
            var deviceTokens = await _unitOfWork.UserDeviceTokens.ListAsync(tokensSpec, cancellationToken);
            var tokens = deviceTokens.Select(t => t.Token).ToList();

            if (tokens.Count > 0)
            {
                var notificationRequest = new PushNotificationRequest
                {
                    Title = "Subscriptions",
                    Body = $"A new subscription has been added for you at {notification.BranchName}."
                };

                await _pushNotificationService.SendToDevicesAsync(notificationRequest, tokens, cancellationToken);
            }
        }
    }
}
