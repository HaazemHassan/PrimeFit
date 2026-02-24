using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Commands.DeleteBranchImage
{
    public class DeleteBranchImageCommandHandler : IRequestHandler<DeleteBranchImageCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;

        public DeleteBranchImageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageService = imageService;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteBranchImageCommand request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserService.UserId!.Value;

            var spec = new BranchWithImageForOwnerSpec(ownerId, request.BranchId, request.ImageId);
            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(spec, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }

            var publicId = branch.Images.FirstOrDefault(i => i.Id == request.ImageId)?.PublicId;
            var deleteResult = branch.RemoveImage(request.ImageId);
            if (deleteResult.IsError)
            {
                return deleteResult.Errors;
            }


            var deleteFromCloudResult = await _imageService.DeleteAsync(publicId!);

            if (deleteFromCloudResult.IsError)
            {
                return deleteFromCloudResult.Errors;
            }

            return Result.Success;

        }


    }
}
