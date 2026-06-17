using ErrorOr;
using MediatR;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    public record RegisterDeviceTokenCommand(
        string Token,
        DevicePlatform DevicePlatform
    ) : IRequest<ErrorOr<Success>>;
}
