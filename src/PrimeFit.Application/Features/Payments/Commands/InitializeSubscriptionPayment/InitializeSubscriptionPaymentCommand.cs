using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment
{
    public class InitializeSubscriptionPaymentCommand : IRequest<ErrorOr<InitializeSubscriptionPaymentCommandResponse>>
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int PackageId { get; set; }
    }
}
