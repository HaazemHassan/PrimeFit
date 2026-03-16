using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage
{
    public class UpdateBranchImageCommandHandler : IRequestHandler<UpdateBranchImageCommand, ErrorOr<UpdateBranchImageCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;
        private readonly IBranchAuthorizationService _branchAuthorizationService;


        public UpdateBranchImageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageService imageService,
            IImageBackgroundService imageBackgroundQueue,
            IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageService = imageService;
            _branchAuthorizationService = branchAuthorizationService;
        }


        public async Task<ErrorOr<UpdateBranchImageCommandResponse>> Handle(UpdateBranchImageCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.BranchImagesWrite, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var image = await _unitOfWork.BranchImages.GetAsync(
                i => i.BranchId == request.BranchId &&
                     i.Id == request.ImageId,
                cancellationToken);

            if (image is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.ImageNotFound,
                    "Branch image not found");
            }

            var generatedFileName =
                $"branch-{request.BranchId}-{image.Type}-{Guid.NewGuid()}";

            var replaceResult = await _imageService.ReplaceAsync(
                request.ImageStream,
                image.PublicId,
                generatedFileName,
                cancellationToken
            );

            if (replaceResult.IsError)
            {
                return replaceResult.Errors;

            }

            var updatedData = replaceResult.Value;

            var domainResult = image.UpdateImage(updatedData.SecureUrl, updatedData.PublicId);

            if (domainResult.IsError)
            {
                return domainResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateBranchImageCommandResponse(image.Id, image.Url);
        }
    }
}
