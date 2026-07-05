namespace PrimeFit.Api.Requests.Branches.UpdateBranchImage {
    public class UpdateBranchImageRequest
    {
        public IFormFile ImageFile { get; set; } = null!;
    }
}
