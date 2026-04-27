using PrimeFit.Domain.Common.Enums;
﻿using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.MembersWrite])]
    public class CreateMemberWithSubscriptionCommand : IRequest<ErrorOr<CreateMemberWithSubscriptionCommandResponse>>,
        IBranchAuthorizedRequest

    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
    }
}
