using ErrorOr;

namespace PrimeFit.Api.Requests.InAppNotifications.GetMyNotifications;

public class GetMyNotificationsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}
