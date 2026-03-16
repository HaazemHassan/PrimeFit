using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Requests;
using PrimeFit.API.Requests.Branches;
using PrimeFit.API.Requests.Branches.AddBranchImage;
using PrimeFit.API.Requests.Branches.Subscriptions;
using PrimeFit.API.Requests.Branches.UpdateBranchImage;
using PrimeFit.Application.Features.Branches.Commands.CreateBranch;
using PrimeFit.Application.Features.Branches.Commands.CreateBranchImage;
using PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription;
using PrimeFit.Application.Features.Branches.Commands.DeleteBranchImage;
using PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage;
using PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;
using PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails;
using PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics;
using PrimeFit.Application.Features.BranchPackages.Commands.AddPackage;
using PrimeFit.Application.Features.BranchPackages.Commands.DeletePackage;
using PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage;
using PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackageStatus;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;
using PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions;

namespace PrimeFit.API.Controllers.Owner
{
    public class BranchesController : OwnerBaseController
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AddBranchImageRequest> _addBranchImageValidator;
        private readonly IValidator<UpdateBranchImageRequest> _updateLocationDetailsValidator;

        public BranchesController(IMapper mapper, IValidator<AddBranchImageRequest> addBranchImageValidator, IValidator<UpdateBranchImageRequest> updateLocationDetailsValidator)
        {
            _mapper = mapper;
            _addBranchImageValidator = addBranchImageValidator;
            _updateLocationDetailsValidator = updateLocationDetailsValidator;
        }

        [HttpPost()]
        public async Task<IActionResult> AddBranch([FromBody] CreateBranchCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { branchId = result.Value.Id }, result.Value);
        }



        [HttpPatch("{branchId:int}/bussiness-details")]
        public async Task<IActionResult> UpdateBussinessDetails([FromRoute] int branchId, [FromBody] UpdateBussinessDetailsRequest request)
        {

            var command = _mapper.Map<UpdateBussinessDetailsCommand>(request);
            command.BranchId = branchId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }


        [HttpPatch("{branchId:int}/location-details")]
        public async Task<IActionResult> UpdateLocationDetails([FromRoute] int branchId, [FromBody] UpdateLocationDetailsRequest request)
        {

            var command = _mapper.Map<UpdateLocationDetailsCommand>(request);
            command.BranchId = branchId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }

        [HttpPatch("{branchId:int}/working-hours")]
        public async Task<IActionResult> UpdateWorkingHoursDetails([FromRoute] int branchId, [FromBody] UpdateWorkingHoursRequest request)
        {

            var command = _mapper.Map<UpdateWorkingHoursCommand>(request);
            command.BranchId = branchId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPatch("{branchId:int}/status")]
        public async Task<IActionResult> UpdateBranchStatus([FromRoute] int branchId, [FromBody] UpdateBranchStatusRequest request)
        {
            var command = new UpdateBranchStatusCommand
            {
                BranchId = branchId,
                BranchStatus = request.BranchStatus
            };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }



        [HttpGet("{branchId:int}/setup-details")]
        public async Task<IActionResult> GetBranchSetupDetails([FromRoute] int branchId, [FromQuery] GetBranchSetupDetailsRequest request)
        {
            var query = _mapper.Map<GetBranchSetupDetailsQuery>(request);
            query.BranchId = branchId;

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }


        [HttpPost("{branchId:int}/images")]
        public async Task<IActionResult> AddBranchImage([FromRoute] int branchId, [FromForm] AddBranchImageRequest request)
        {
            var validationResult = await _addBranchImageValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation Exception", validationResult.Errors);
            }

            using var stream = request.ImageFile.OpenReadStream();

            var command = new AddBranchImageCommand
            {
                BranchId = branchId,
                ImageStream = stream,
                ImageType = request.ImageType
            };


            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);

        }

        [HttpPatch("{branchId:int}/images/{imageId}")]
        public async Task<IActionResult> UpdateBranchImage(int branchId, int imageId, UpdateBranchImageRequest request)
        {
            var validationResult = await _updateLocationDetailsValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException("Validation Exception", validationResult.Errors);
            }

            using var stream = request.ImageFile.OpenReadStream();

            var command = new UpdateBranchImageCommand
            {
                BranchId = branchId,
                ImageId = imageId,
                ImageStream = stream,
            };


            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);

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

        [HttpPost("{branchId:int}/packages")]
        public async Task<IActionResult> AddPackage([FromRoute] int branchId, [FromBody] AddPackageRequest request)
        {
            var command = _mapper.Map<AddPackageCommand>(request);
            command.BranchId = branchId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpPatch("{branchId:int}/packages/{packageId:int}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int branchId, [FromRoute] int packageId, [FromBody] UpdatePackageRequest request)
        {
            var command = _mapper.Map<UpdatePackageCommand>(request);
            command.BranchId = branchId;
            command.PackageId = packageId;

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }


        [HttpPatch("{branchId:int}/packages/{packageId:int}/status")]
        public async Task<IActionResult> UpdatePackageStatus([FromRoute] int branchId, [FromRoute] int packageId, [FromBody] UpdatePackageStatusRequest request)
        {
            var command = new UpdatePackageStatusCommand
            {
                BranchId = branchId,
                PackageId = packageId,
                IsActive = request.IsActive
            };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }

        [HttpDelete("{branchId:int}/packages/{packageId:int}")]
        public async Task<IActionResult> DeletePackage([FromRoute] int branchId, [FromRoute] int packageId)
        {
            var command = new DeletePackageCommand
            {
                BranchId = branchId,
                PackageId = packageId
            };

            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return NoContent();
        }


        [HttpGet("{branchId:int}/packages")]
        public async Task<IActionResult> GetBranchPackages([FromRoute] int branchId, [FromQuery] BasicPaginationRequest request)
        {

            var query = _mapper.Map<GetBranchPackagesQuery>(request);
            query.BranchId = branchId;

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }




        [HttpPost("{branchId:int}/members")]
        public async Task<IActionResult> CreateMemberWithSubscription([FromRoute] int branchId, [FromBody] CreateMemberWithSubscriptionRequest request)
        {

            var command = _mapper.Map<CreateMemberWithSubscriptionCommand>(request);
            command.BranchId = branchId;
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }


            //i will update this later
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { branchId = result.Value.Id }, result.Value);
        }



        [HttpPost("{branchId:int}/subscriptions")]
        public async Task<IActionResult> AddSubscription([FromRoute] int branchId, [FromBody] AddSubscriptionRequest request)
        {

            var command = _mapper.Map<AddSubscriptionCommand>(request);
            command.BranchId = branchId;
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }


            //i will update this lateer
            return CreatedAtRoute(RouteNames.Branches.GetBranchById, new { branchId = result.Value.Id }, result.Value);
        }


        [HttpGet("{branchId:int}/subscriptions")]
        public async Task<IActionResult> GetSubscriptions([FromRoute] int branchId, [FromQuery] GetBranchSubscriptionsRequest request)
        {

            var query = _mapper.Map<GetBranchSubscriptionsQuery>(request);
            query.BranchId = branchId;

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }

        [HttpGet("{branchId:int}/statistics")]
        public async Task<IActionResult> GetBranchStatistics([FromRoute] int branchId, [FromBody] GetBranchStatisticsRequest request)
        {
            var query = new GetBranchStatisticsQuery(branchId)
            {
                TimePeriod = request.TimePeriod
            };

            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }
            return Ok(result.Value);
        }







    }
}