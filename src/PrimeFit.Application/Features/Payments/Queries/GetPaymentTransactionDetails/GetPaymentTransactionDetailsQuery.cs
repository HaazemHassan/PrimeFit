using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Payments.Queries.GetPaymentTransactionDetails
{
    public class GetPaymentTransactionDetailsQuery : IRequest<ErrorOr<GetPaymentTransactionDetailsQueryResponse>>
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
    }
}
