using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionsDetailsQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, ErrorOr<GetSubscriptionByIdQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public GetSubscriptionsDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDateTimeProvider dateTimeProvider, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _branchAuthorizationService = branchAuthorizationService;
        }


        public async Task<ErrorOr<GetSubscriptionByIdQueryResponse>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var subscriptionSpec = new SubscriptionWithFullDetailsSpec(request.SubscriptionId, null, null);
            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(subscriptionSpec, cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
            }

            var authResult = await _branchAuthorizationService.AuthorizeAsync(subscription.BranchId, Permission.SubscriptionsView, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var response = _mapper.Map<GetSubscriptionByIdQueryResponse>(subscription);

            var localNow = _dateTimeProvider.GetNow("Africa/Cairo");
            response.RemainingDays = subscription.GetRemainingDays(localNow);

            return response;
        }
    }
}
