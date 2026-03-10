using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;

namespace PrimeFit.API.Controllers
{
    public class EmployeesController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreateEmployeeCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return Created(string.Empty, result.Value);
        }
    }
}
