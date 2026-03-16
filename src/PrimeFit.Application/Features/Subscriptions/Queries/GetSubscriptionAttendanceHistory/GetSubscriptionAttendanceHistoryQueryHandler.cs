using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Extensions;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory
{
    public class GetSubscriptionAttendanceHistoryQueryHandler
          : IRequestHandler<GetSubscriptionAttendanceHistoryQuery, ErrorOr<GetSubscriptionAttendanceHistoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<CheckIn> _checkInRepository;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetSubscriptionAttendanceHistoryQueryHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<CheckIn> checkInRepository,
            IBranchAuthorizationService branchAuthorizationService,
            IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _checkInRepository = checkInRepository;
            _branchAuthorizationService = branchAuthorizationService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<GetSubscriptionAttendanceHistoryResponse>> Handle(
            GetSubscriptionAttendanceHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions
                .GetByIdAsync(request.SubscriptionId, cancellationToken);

            if (subscription == null)
            {
                return Error.NotFound("Subscription.NotFound", "The subscription was not found.");
            }

            var branchNow = _dateTimeProvider.GetTimeZoneNow();
            var requestedYear = request.Year ?? branchNow.Year;
            var requestedMonth = request.Month ?? branchNow.Month;

            if (subscription.Status == SubscriptionStatus.Scheduled)
            {
                return CreateEmptyResponse(subscription, requestedYear, requestedMonth);
            }

            var requestedMonthStart = new DateTimeOffset(
                requestedYear,
                requestedMonth,
                1,
                0, 0, 0,
                branchNow.Offset);

            var requestedMonthNextStart = requestedMonthStart.AddMonths(1);

            var attendedDays = await _unitOfWork.CheckIns.GetAttendedDaysAsync(
                request.SubscriptionId,
                requestedMonthStart,
                requestedMonthNextStart,
                cancellationToken);

            var currentMonthStart = branchNow.StartOfMonth();

            var subscriptionCreatedAt = _dateTimeProvider.ConvertToTimeZone(subscription.CreatedAt);

            DateTimeOffset? subscriptionEndAt = null;

            if (subscription.EndDate.HasValue)
            {
                subscriptionEndAt = _dateTimeProvider.ConvertToTimeZone(subscription.EndDate.Value);
            }

            var subscriptionStartMonth = subscriptionCreatedAt.StartOfMonth();

            DateTimeOffset? subscriptionEndMonth = subscriptionEndAt?.StartOfMonth();

            var hasPreviousMonth = requestedMonthStart > subscriptionStartMonth;

            var hasNextMonth = (subscriptionEndMonth == null || requestedMonthNextStart <= subscriptionEndMonth) &&
                requestedMonthNextStart <= currentMonthStart;

            return new GetSubscriptionAttendanceHistoryResponse
            {
                SubscriptionId = subscription.Id,
                CreatedAt = subscription.CreatedAt,
                Year = requestedYear,
                Month = requestedMonth,
                AttendedDays = attendedDays,
                HasPreviousMonth = hasPreviousMonth,
                HasNextMonth = hasNextMonth
            };
        }

        private static GetSubscriptionAttendanceHistoryResponse CreateEmptyResponse(
            Subscription sub,
            int year,
            int month)
        {
            return new GetSubscriptionAttendanceHistoryResponse
            {
                SubscriptionId = sub.Id,
                CreatedAt = sub.CreatedAt,
                Year = year,
                Month = month,
                AttendedDays = [],
                HasPreviousMonth = false,
                HasNextMonth = false
            };
        }
    }
}