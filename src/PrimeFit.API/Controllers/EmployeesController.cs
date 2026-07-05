using AutoMapper;
using PrimeFit.Api.Requests.Branches.AddBranchImage;
using PrimeFit.Api.Requests.Branches.UpdateBranchImage;
using PrimeFit.Api.Requests.Branches.GetBranchById;
using PrimeFit.Api.Requests.Branches.ActivateBranchImages;
using PrimeFit.Api.Requests.Branches.GetBranchStatistics;
using PrimeFit.Api.Requests.Branches.GetOwnerBranchesStatistics;
using PrimeFit.Api.Requests.Branches.UpdateBranchStatus;
using PrimeFit.Api.Requests.Branches.UpdateBasicDetails;
using PrimeFit.Api.Requests.Branches.UpdateLocationDetailsRequest;
using PrimeFit.Api.Requests.Branches.UpdateWorkingHoursRequest;
using PrimeFit.Api.Requests.BranchPackages.AddPackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackageStatus;
using PrimeFit.Api.Requests.BranchPackages.GetBranchPackagesForCustomers;
using PrimeFit.Api.Requests.Employees.GetBranchEmployees;
using PrimeFit.Api.Requests.Employees.UpdateEmployeeRequest;
using PrimeFit.Api.Requests.Subscriptions.AddSubscription;
using PrimeFit.Api.Requests.Subscriptions.CreateMemberWithSubscription;
using PrimeFit.Api.Requests.Subscriptions.GetBranchSubscriptions;
using PrimeFit.Api.Requests.Subscriptions.GetSubscriptionAttendanceHistory;
using PrimeFit.Api.Requests.Common.Pagination;
using PrimeFit.Api.Requests.Users.UpdateProfile;
using PrimeFit.Api.Requests.BranchReviews;
using PrimeFit.Api.Requests.BranchReviews.GetBranchReviewsRequest;
using PrimeFit.Api.Requests.Notifications;
using PrimeFit.Api.Requests.Payments;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;
using PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles;
using PrimeFit.Api.Requests.Employees.CreateEmployee;

namespace PrimeFit.API.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IMapper _mapper;

        public EmployeesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetEmployeeRoles()
        {
            var result = await Mediator.Send(new GetEmployeeRolesQuery());

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            var command = _mapper.Map<CreateEmployeeCommand>(request);
            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return Created(string.Empty, result.Value);
        }

        [HttpPatch("{employeeId:int}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int employeeId, [FromBody] UpdateEmployeeRequest request)
        {
            var command = _mapper.Map<UpdateEmployeeCommand>(request);
            command.EmployeeId = employeeId;

            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return Ok(result.Value);
        }
    }
}
