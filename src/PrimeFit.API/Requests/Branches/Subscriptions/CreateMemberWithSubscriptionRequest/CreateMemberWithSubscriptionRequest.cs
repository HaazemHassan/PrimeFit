namespace PrimeFit.API.Requests.Branches.Subscriptions
{
    public class CreateMemberWithSubscriptionRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int PackageId { get; set; }
    }
}
