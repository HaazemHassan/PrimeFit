using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Requests.Owner.Branches;
using PrimeFit.Application.Features.Branches.Commands.AddBranchBussinessDetails;
using PrimeFit.Application.Features.Branches.Commands.AddWorkingHours;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;

namespace PrimeFit.API.Controllers.Owner
{
    public class BranchesController : OwnerBaseController
    {
        private readonly IMapper _mapper;

        public BranchesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<IActionResult> AddBranch([FromBody] AddBranchCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { id = result.Value }, null);
        }


        [HttpPatch("{id:int}/location-details")]
        public async Task<IActionResult> UpdateLocationDetails([FromRoute] int id, [FromBody] UpdateLocationDetailsRequest request)
        {

            var command = _mapper.Map<UpdateLocationDetailsCommand>(request);
            command.BranchId = id;
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }

        [HttpPatch("{id:int}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHoursDetails([FromRoute] int id, [FromBody] UpdateWorkingHoursRequest request)
        {

            var command = _mapper.Map<UpdateWorkingHoursCommand>(request);
            command.BranchId = id;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

    }
}
