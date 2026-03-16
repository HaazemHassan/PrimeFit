using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptionById
{
    public class GetMySubscriptionByIdQueryHandler(
        IUnitOfWork _unitOfWork,
        ICurrentUserService _currentUserService,
        IGenericRepository<CheckIn> _checkInRepository)
        : IRequestHandler<GetMySubscriptionByIdQuery, ErrorOr<GetMySubscriptionByIdResponse>>
    {
        public async Task<ErrorOr<GetMySubscriptionByIdResponse>> Handle(GetMySubscriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId!.Value;

            var response = await _unitOfWork.Subscriptions.GetAsync<GetMySubscriptionByIdResponse>(
                s => s.Id == request.SubscriptionId && s.UserId == userId,
                cancellationToken);

            if (response is null)
            {
                return Error.NotFound(ErrorCodes.Subscription.NotFound, "Subscription not found");

            }

            response.CheckInsCount = await _checkInRepository.CountAsync(c => c.SubscriptionId == response.SubscriptionId && c.CustomerId == userId, cancellationToken);

            return response;
        }
    }
}