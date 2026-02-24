using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchImage
{
    public class AddBranchImageCommandHandler : IRequestHandler<AddBranchImageCommand, ErrorOr<AddBranchImageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;

        public AddBranchImageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageService = imageService;
        }

        public async Task<ErrorOr<AddBranchImageCommandResponse>> Handle(AddBranchImageCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId!.Value;


            var branchWithImagesSpec = new BranchWithImagesSpec(currentUserId, request.BranchId);
            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(branchWithImagesSpec, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }


            //double check here also before uploding the image -- also there is a check in the domain
            var canAddImageResult = branch.CanAddImage(request.ImageType);
            if (canAddImageResult.IsError)
                return canAddImageResult.Errors;

            var generatedFileName = $"branch-{request.BranchId}-{request.ImageType}-{Guid.NewGuid()}";

            var imageUploadResult = await _imageService.UploadAsync(request.ImageStream, generatedFileName);

            if (imageUploadResult.IsError)
            {
                return imageUploadResult.Errors;
            }


            var uploadedImageData = imageUploadResult.Value;

            var addImageToBranchResult = branch.AddImage(
                uploadedImageData.SecureUrl,
                uploadedImageData.PublicId,
                request.ImageType);

            if (addImageToBranchResult.IsError)
            {
                await _imageService.DeleteAsync(uploadedImageData.PublicId);
                return addImageToBranchResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddBranchImageCommandResponse(uploadedImageData.SecureUrl);
        }

    }
}
