using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionsDetailsQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, ErrorOr<GetSubscriptionByIdQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        public GetSubscriptionsDetailsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }



        public async Task<ErrorOr<GetSubscriptionByIdQueryResponse>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            int ownerId = _currentUserService.UserId!.Value;


            var subscriptionSpec = new SubscriptionWithFullDetailsSpec(request.SubscriptionId, ownerId, null);
            var subscription = await _unitOfWork.Subscriptions.FirstOrDefaultAsync(subscriptionSpec, cancellationToken);

            if (subscription is null)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");
            }

            var response = _mapper.Map<GetSubscriptionByIdQueryResponse>(subscription);

            var localNow = _dateTimeProvider.GetNow("Africa/Cairo");
            response.RemainingDays = subscription.GetRemainingDays(localNow);

            return response;
        }
    }
}
