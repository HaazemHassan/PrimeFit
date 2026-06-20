namespace PrimeFit.API.Requests.Payments
{
    public class InitializeSubscriptionPaymentRequest
    {
        public int PackageId { get; set; }
        public int BranchId { get; set; }
    }
}
