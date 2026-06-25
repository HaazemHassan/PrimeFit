using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyUnreadNotifications
{
    [Authorize]
    public class GetMyUnreadNotificationsQuery
        : IRequest<ErrorOr<List<InAppNotificationDto>>>
        , IAuthorizedRequest
    {
    }
}
