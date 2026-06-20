using ErrorOr;
using MediatR;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Payments.Queries.GetPaymentTransactionDetails
{
    public class GetPaymentTransactionDetailsQueryHandler : IRequestHandler<GetPaymentTransactionDetailsQuery, ErrorOr<GetPaymentTransactionDetailsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPaymentTransactionDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetPaymentTransactionDetailsQueryResponse>> Handle(GetPaymentTransactionDetailsQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.PaymentTransactions.GetAsync(
                t => t.Id == request.TransactionId && t.UserId == request.UserId,
                cancellationToken);

            if (transaction is null)
            {
                return Error.NotFound(
                    ErrorCodes.Payment.TransactionNotFound,
                    "Payment transaction not found");
            }

            return new GetPaymentTransactionDetailsQueryResponse
            {
                TransactionId = transaction.Id,
                Status = transaction.Status,
                PaidAt = transaction.PaidAt,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                PackageId = transaction.PackageId
            };
        }
    }
}
