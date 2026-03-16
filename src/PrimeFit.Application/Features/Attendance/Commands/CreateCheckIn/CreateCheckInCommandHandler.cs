using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn.Specs;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn
{
    public class CreateCheckInCommandHandler(
        IUnitOfWork _unitOfWork,
        IGenericRepository<CheckIn> _checkInRepository,
        ITotpService _totpService,
        IBranchAuthorizationService _branchAuthorizationService)
        : IRequestHandler<CreateCheckInCommand, ErrorOr<CreateCheckInCommandResponse>>
    {
        public async Task<ErrorOr<CreateCheckInCommandResponse>> Handle(CreateCheckInCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.CheckInWrite, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }


            var spec = new SubscriptionForCheckInSpec(request.CustomerId, request.BranchId);
            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(
                  code: ErrorCodes.CheckIn.SubscriptionNotFound,
                  description: "Not member.");
            }

            var canCheckInResult = subscription.EnsureCanCheckIn();

            if (canCheckInResult.IsError)
            {
                return canCheckInResult.Errors;
            }

            var customer = subscription.User;

            if (customer.TotpSecret is null || !_totpService.VerifyTotpCode(customer.TotpSecret, request.Code))
            {
                return Error.Validation(
                   code: ErrorCodes.CheckIn.InvalidCode,
                   description: "Invalid check-in code.");
            }


            var checkIn = new CheckIn
            {
                CustomerId = request.CustomerId,
                BranchId = request.BranchId,
                SubscriptionId = subscription.Id
            };

            await _checkInRepository.AddAsync(checkIn, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            var lastCheckInDateSpec = new CustomerLastCheckInDateSpec(request.CustomerId, request.BranchId);
            var lastCheckIn = await _checkInRepository.FirstOrDefaultAsync(lastCheckInDateSpec, cancellationToken);

            return new CreateCheckInCommandResponse
            {
                MemberName = customer.FullName,
                PackageName = subscription.Package.Name,
                LastCheckIn = lastCheckIn
            };
        }
    }
}

