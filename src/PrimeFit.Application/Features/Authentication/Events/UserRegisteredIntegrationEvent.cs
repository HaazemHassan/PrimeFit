using PrimeFit.Application.Common.Messaging;

namespace PrimeFit.Application.Features.Authentication.Events
{
    public sealed record UserRegisteredIntegrationEvent(string Email) : IIntegrationEvent;
}
