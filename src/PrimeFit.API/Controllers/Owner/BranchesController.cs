using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Requests.Owner.Branches;
using PrimeFit.API.Requests.Owner.Branches.AddBranchImage;
using PrimeFit.Application.Features.Branches.Commands.AddBranchBussinessDetails;
using PrimeFit.Application.Features.Branches.Commands.AddBranchImage;
using PrimeFit.Application.Features.Branches.Commands.AddWorkingHours;
using PrimeFit.Application.Features.Branches.Commands.DeleteBranchImage;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;

namespace PrimeFit.API.Controllers.Owner
{
    public class BranchesController : OwnerBaseController
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AddBranchImageRequest> _addBranchImageValidator;

        public BranchesController(IMapper mapper, IValidator<AddBranchImageRequest> addBranchImageValidator)
        {
            _mapper = mapper;
            _addBranchImageValidator = addBranchImageValidator;
        }

        [HttpPost()]
        public async Task<IActionResult> AddBranch([FromBody] AddBranchCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { id = result.Value.Id }, result.Value);
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


        [HttpPost("{id:int}/images")]
        public async Task<IActionResult> AddBranchImage([FromRoute] int id, [FromForm] AddBranchImageRequest request)
        {
            var validationResult = await _addBranchImageValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation Exception", validationResult.Errors);
            }

            using var stream = request.File.OpenReadStream();

            var command = new AddBranchImageCommand
            {
                BranchId = id,
                ImageStream = stream,
                ImageType = request.ImageType
            };


            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();

        }


        [HttpDelete("{branchId:int}/images/{imageId}")]
        public async Task<IActionResult> DeleteBranchImage(int branchId, int imageId)
        {


            var command = new DeleteBranchImageCommand
            {
                BranchId = branchId,
                ImageId = imageId
            };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();

        }
    }
}