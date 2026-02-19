using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Users.Commands.UpdateProfile
{
    [Authorize(Policy = AuthorizationPolicies.SelfOrAdmin)]
    public class UpdateProfileCommand : IRequest<ErrorOr<UpdateProfileCommandResponse>>, IOwnedResourceRequest
    {
        public int OwnerUserId { get; set; }  // Used for authorization policy
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
}
