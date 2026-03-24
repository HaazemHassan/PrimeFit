using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Requests.Employees.UpdateEmployeeRequest;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;

namespace PrimeFit.API.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IMapper _mapper;

        public EmployeesController(IMapper mapper)
        {
            _mapper = mapper;
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
