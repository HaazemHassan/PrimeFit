using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class UpdateLocationDetailsCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int GovernorateId { get; set; }
        public string Address { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
