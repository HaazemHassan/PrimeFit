namespace PrimeFit.API.Requests.Owner.Branches
{
    public class UpdateLocationDetailsRequest
    {
        public int GovernorateId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
