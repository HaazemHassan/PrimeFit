using ErrorOr;

namespace PrimeFit.Api.Requests.Attendance.CreateCheckIn;

public class CreateCheckInRequest
{
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public int BranchId { get; set; }
}
