namespace PrimeFit.Infrastructure.Data.Seeding
{
    public class EmployeeRoleSeedDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
