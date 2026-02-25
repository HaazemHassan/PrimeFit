namespace PrimeFit.API.Requests.Owner.Branches
{
    public class UpdateLocationDetailsRequest
    {
        public int GovernorateId { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
