using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQuery : IRequest<ErrorOr<CheckEmailAvailabilityQueryResponse>> {
        public string Email { get; set; }
    }
}
