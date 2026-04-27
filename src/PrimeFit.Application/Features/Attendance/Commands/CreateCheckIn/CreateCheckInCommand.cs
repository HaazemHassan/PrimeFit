using PrimeFit.Domain.Common.Enums;
﻿using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.CheckInWrite])]
    public class CreateCheckInCommand : IRequest<ErrorOr<CreateCheckInCommandResponse>>, IBranchAuthorizedRequest
    {
        public int CustomerId { get; set; }
        public string Code { get; set; }
        public int BranchId { get; set; }
    }
}
