namespace PrimeFit.Application.Features.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandResponse
    {
        public CreateBranchCommandResponse(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
