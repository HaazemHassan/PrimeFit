using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.Employees.UpdateEmployeeRequest;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;
using PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles;

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
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
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
