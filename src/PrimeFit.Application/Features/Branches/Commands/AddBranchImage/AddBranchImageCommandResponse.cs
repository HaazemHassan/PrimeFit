namespace PrimeFit.Application.Features.Branches.Commands.AddBranchImage
{
    public class AddBranchImageCommandResponse
    {
        public string ImageUrl { get; set; }

        public AddBranchImageCommandResponse(string imageUrl)
        {
            ImageUrl = imageUrl;
        }
    }
}
