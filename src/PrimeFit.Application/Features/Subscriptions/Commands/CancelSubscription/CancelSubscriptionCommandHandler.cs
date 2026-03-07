using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CancelSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<Success>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            int curUserId = _currentUserService.UserId!.Value;

            var spec = new SubscriptionWithBranchSpec(request.SubscriptionId);

            var subscriptionToCancel = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscriptionToCancel is null || subscriptionToCancel.Branch.OwnerId != curUserId)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
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
            return Result.Success;

        }

    }
}
