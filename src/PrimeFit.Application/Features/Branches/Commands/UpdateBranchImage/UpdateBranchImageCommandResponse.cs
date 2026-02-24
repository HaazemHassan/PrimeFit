namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage
{
    public class UpdateBranchImageCommandResponse
    {

        public UpdateBranchImageCommandResponse(int imageId, string imageUrl)
        {
            ImageId = imageId;

            ImageUrl = imageUrl;
        }

        public int ImageId { get; set; }
        public string ImageUrl { get; set; }


    }
}
