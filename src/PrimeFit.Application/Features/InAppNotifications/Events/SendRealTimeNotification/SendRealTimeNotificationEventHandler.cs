using AutoMapper;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.InAppNotifications.Events.SendRealTimeNotification
{
    public class SendRealTimeNotificationEventHandler : INotificationHandler<SendRealTimeNotificationEvent>
    {
        private readonly INotificationHelperService _notificationHelper;
        private readonly IMapper _mapper;

        public SendRealTimeNotificationEventHandler(INotificationHelperService notificationHelper, IMapper mapper)
        {
            _notificationHelper = notificationHelper;
            _mapper = mapper;
        }

        public async Task Handle(SendRealTimeNotificationEvent notificationEvent, CancellationToken cancellationToken)
        {
            var dto = _mapper.Map<InAppNotificationDto>(notificationEvent.Notification);
            await _notificationHelper.PushRealTimeAsync(notificationEvent.Notification.UserId, dto);
        }
    }
}
