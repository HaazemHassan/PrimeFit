using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Commands.FreezeSubscription
{
    public class FreezeSubscriptionCommandHandler : IRequestHandler<FreezeSubscriptionCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;

        public FreezeSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, TimeProvider timeProvider)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _timeProvider = timeProvider;
        }

        public async Task<ErrorOr<Success>> Handle(FreezeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            int curUserId = _currentUserService.UserId!.Value;

            var spec = new SubscriptionWithBranchSpec(request.SubscriptionId);

            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(spec, cancellationToken);

            if (subscription is null || subscription.Branch.OwnerId != curUserId)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
            }


            var freezeResult = subscription.Freeze(_timeProvider.GetUtcNow());
            if (freezeResult.IsError)
            {
                return freezeResult.Errors;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success;

        }

    }
}
