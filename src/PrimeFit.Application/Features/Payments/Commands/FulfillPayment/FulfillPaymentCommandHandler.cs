using ErrorOr;
using MediatR;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Payments.Commands.FulfillPayment
{
    public class FulfillPaymentCommandHandler : IRequestHandler<FulfillPaymentCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISubscriptionDomainService _subscriptionDomainService;

        public FulfillPaymentCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            ISubscriptionDomainService subscriptionDomainService)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _subscriptionDomainService = subscriptionDomainService;
        }

        public async Task<ErrorOr<Success>> Handle(FulfillPaymentCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.PaymentTransactions.GetAsync(
                t => t.StripePaymentIntentId == request.StripePaymentIntentId,
                cancellationToken);

            if (transaction is null)
            {
                return Error.NotFound(
                    ErrorCodes.Payment.TransactionNotFound,
                    "Payment transaction not found.");
            }

            if (transaction.Status != PaymentTransactionStatus.Pending)
            {
                return Error.Conflict(
                    ErrorCodes.Payment.TransactionAlreadyProcessed,
                    "This payment transaction has already been processed.");
            }

            var now = _dateTimeProvider.UtcNow;

            var markResult = transaction.MarkAsSucceeded(now);

            if (markResult.IsError)
            {
                return markResult.Errors;
            }

            var user = await _unitOfWork.Users.GetByIdAsync(transaction.UserId, cancellationToken);
            if (user is null)
            {
                return Error.NotFound("User.NotFound", "User not found");

            }

            var package = await _unitOfWork.Packages.GetByIdAsync(transaction.PackageId, cancellationToken);
            if (package is null)
            {
                return Error.NotFound("Package.NotFound", "Package not found");
            }

            var branch = await _unitOfWork.Branches.GetByIdAsync(package.BranchId, cancellationToken);
            if (branch is null)
            {
                return Error.NotFound("Branch.NotFound", "Branch not found");
            }

            var subscriptionResult = await _subscriptionDomainService.CreateSubscriptionAsync(user, branch, package, cancellationToken);
            if (subscriptionResult.IsError)
            {
                return subscriptionResult.Errors;
            }

            var subscription = subscriptionResult.Value;

            await _unitOfWork.Subscriptions.AddAsync(subscription, cancellationToken);

            return Result.Success;
        }
    }
}
