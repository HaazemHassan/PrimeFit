using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyNotifications
{
    [Authorize]
    public class GetMyNotificationsQuery
        : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<InAppNotificationDto>>>
        , IAuthorizedRequest
    {
    }
}
