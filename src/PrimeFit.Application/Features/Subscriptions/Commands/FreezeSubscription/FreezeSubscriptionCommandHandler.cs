using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    public class FreezeSubscriptionCommandHandler : IRequestHandler<FreezeSubscriptionCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        private readonly ICurrentUserService _currentUserService;

        public FreezeSubscriptionCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IBranchAuthorizationService branchAuthorizationService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _branchAuthorizationService = branchAuthorizationService;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Success>> Handle(FreezeSubscriptionCommand request, CancellationToken cancellationToken)
        {

            var curUserId = _currentUserService.UserId;

            var spec = new SubscriptionWithBranchAndFreezesSpec(request.SubscriptionId);

            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
            }

            if (subscription.UserId != curUserId)
            {
                var authResult = await _branchAuthorizationService.AuthorizeAsync(
                   subscription.BranchId,
                   Permission.SubscriptionsWrite,
                   cancellationToken);

                if (authResult.IsError)
                {
                    return authResult.Errors;
                }
            }



            var freezeResult = subscription.Freeze(_dateTimeProvider.UtcNow);
            if (freezeResult.IsError)
            {
                return freezeResult.Errors;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success;

        }

    }
}
