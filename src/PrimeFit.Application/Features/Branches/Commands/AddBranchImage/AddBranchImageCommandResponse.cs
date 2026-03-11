namespace PrimeFit.Application.Features.Branches.Commands.CreateBranchImage
{
    public class AddBranchImageCommandResponse
    {
        public AddBranchImageCommandResponse(int imageId, string imageUrl)
        {
            ImageId = imageId;

            ImageUrl = imageUrl;
        }


        public int ImageId { get; set; }
        public string ImageUrl { get; set; }


    }
}
