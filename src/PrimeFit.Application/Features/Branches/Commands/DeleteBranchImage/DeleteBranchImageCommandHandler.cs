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
        private readonly IImageBackgroundService _imageBackgroundQueue;


        public DeleteBranchImageCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IImageService imageService,
            IImageBackgroundService imageBackgroundQueue)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _imageService = imageService;
            _imageBackgroundQueue = imageBackgroundQueue;
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

            var deleteResult = branch.DeleteImage(request.ImageId);
            if (deleteResult.IsError)
            {
                return deleteResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var publicId = deleteResult.Value;
            _imageBackgroundQueue.DeleteImage(publicId);

            return Result.Success;

        }


    }
}
