namespace PrimeFit.Application.Features.Branches.Commands.AddBranch
{
    public class AddBranchCommandResponse
    {
        public AddBranchCommandResponse(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
