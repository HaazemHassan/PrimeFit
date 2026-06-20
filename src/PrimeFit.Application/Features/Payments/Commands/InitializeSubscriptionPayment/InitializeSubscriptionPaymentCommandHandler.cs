using ErrorOr;
using MediatR;
using PrimeFit.Application.ServicesContracts.Infrastructure.Payments;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment
{
    public class InitializeSubscriptionPaymentCommandHandler : IRequestHandler<InitializeSubscriptionPaymentCommand, ErrorOr<InitializeSubscriptionPaymentCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _stripePaymentService;

        public InitializeSubscriptionPaymentCommandHandler(IUnitOfWork unitOfWork, IPaymentService stripePaymentService)
        {
            _unitOfWork = unitOfWork;
            _stripePaymentService = stripePaymentService;
        }

        public async Task<ErrorOr<InitializeSubscriptionPaymentCommandResponse>> Handle(InitializeSubscriptionPaymentCommand request, CancellationToken cancellationToken)
        {
            var package = await _unitOfWork.Packages.GetAsync(
                p => p.Id == request.PackageId && p.BranchId == request.BranchId && p.IsActive,
                cancellationToken);

            if (package is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.PackageNotFound,
                    "Package not found or inactive.");
            }

            var amountInSmallestUnit = (long)(package.Price * 100);

            var metadata = new Dictionary<string, string>
            {
                ["UserId"] = request.UserId.ToString(),
                ["PackageId"] = package.Id.ToString(),
                ["PaymentType"] = PaymentType.Subscription.ToString()
            };

            var paymentIntentResult = await _stripePaymentService.CreatePaymentIntentAsync(
                amountInSmallestUnit,
                package.Currency,
                metadata,
                cancellationToken);

            if (paymentIntentResult.IsError)
            {
                return paymentIntentResult.Errors;
            }

            var transaction = new PaymentTransaction(
                request.UserId,
                package.Id,
                paymentIntentResult.Value.PaymentIntentId,
                package.Price,
                package.Currency);

            await _unitOfWork.PaymentTransactions.AddAsync(transaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new InitializeSubscriptionPaymentCommandResponse
            {
                ClientSecret = paymentIntentResult.Value.ClientSecret,
                PaymentTransactionId = transaction.Id
            };
        }
    }
}
