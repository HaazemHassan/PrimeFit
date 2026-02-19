using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Users.Queries.GetUserById {
    public class GetUserByIdQueryResponse : UserResponse {

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
