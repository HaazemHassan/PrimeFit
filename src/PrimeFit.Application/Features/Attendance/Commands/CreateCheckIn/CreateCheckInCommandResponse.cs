namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn
{
    public class CreateCheckInCommandResponse
    {
        public string MemberName { get; set; } = null!;
        public string PackageName { get; set; } = null!;
        public DateTimeOffset? LastCheckIn { get; set; }

    }
}