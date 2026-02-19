using ErrorOr;
using MediatR;
using PrimeFit.Domain.Contracts.Repositories;

namespace PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability
{
    public class CheckEmailAvailabilityQueryHandler(IUnitOfWork _unitOfWork)
        : IRequestHandler<CheckEmailAvailabilityQuery, ErrorOr<CheckEmailAvailabilityQueryResponse>>
    {

        public async Task<ErrorOr<CheckEmailAvailabilityQueryResponse>> Handle(CheckEmailAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var emailExists = await _unitOfWork.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            var response = new CheckEmailAvailabilityQueryResponse
            {
                IsAvailable = !emailExists
            };

            return response;
        }
    }
}
