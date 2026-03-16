using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Users.Queries.GetMe
{
    public class GetMeQueryResponse
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public UserRole? UserRole { get; set; }
        public List<BranchLiteDto> Branches { get; set; } = [];
    }



    public class BranchLiteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
