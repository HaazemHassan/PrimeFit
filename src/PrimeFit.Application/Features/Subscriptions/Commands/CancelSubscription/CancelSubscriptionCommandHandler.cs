using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Branches.Caching;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        private readonly ICacheService _cacheService;

        public CancelSubscriptionCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IBranchAuthorizationService branchAuthorizationService,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _branchAuthorizationService = branchAuthorizationService;
            _cacheService = cacheService;
        }

        public async Task<ErrorOr<Success>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var spec = new SubscriptionWithBranchSpec(request.SubscriptionId);

            var subscriptionToCancel = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscriptionToCancel is null)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
            }

            var authResult = await _branchAuthorizationService.AuthorizeAsync(subscriptionToCancel.BranchId, Permission.SubscriptionsCancel, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var subscriptionStatusBeforeCancel = subscriptionToCancel.Status;

            var cancellationResult = subscriptionToCancel.Cancel(_dateTimeProvider.UtcNow);
            if (cancellationResult.IsError)
            {
                return cancellationResult.Errors;
            }


            if (subscriptionStatusBeforeCancel == SubscriptionStatus.Active)
            {
                var scheduledSubscription = await _unitOfWork.Subscriptions.GetAsync(
                    s => s.UserId == subscriptionToCancel.UserId
                    && s.BranchId == subscriptionToCancel.BranchId
                    && s.Status == SubscriptionStatus.Scheduled,
                    cancellationToken);

                if (scheduledSubscription is not null)
                {
                    scheduledSubscription.Activate(_dateTimeProvider.UtcNow);

                }

            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            //Manual invalidation instead of creating invalidation policy because the command doesn't contain BranchId property
            //Update later
            await _cacheService.RemoveByTagAsync(BranchesCache.ByIdTag(subscriptionToCancel.BranchId), cancellationToken);

            return Result.Success;

        }

    }
}
