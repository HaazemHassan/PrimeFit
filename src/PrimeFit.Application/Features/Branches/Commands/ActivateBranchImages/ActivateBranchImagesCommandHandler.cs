using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    public class ActivateBranchImagesCommandHandler : IRequestHandler<ActivateBranchImagesCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IImageService _imageService;
        private readonly IImageBackgroundService _imageBackgroundQueue;


        public ActivateBranchImagesCommandHandler(
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

        public async Task<ErrorOr<Success>> Handle(ActivateBranchImagesCommand request, CancellationToken cancellationToken)
        {

            var spec = new BranchWithActiveAndSelectedPendingImagesSpec(request.BranchId, request.Images);

            var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(spec, cancellationToken);

            if (branch is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound,
                    "Branch not found");
            }



            foreach (var image in branch.Images.Where(i => i.Status == BranchImageStatus.Pending))
            {
                var result = branch.ActivateImage(image);
                if (result.IsError)
                {
                    return result.Errors;
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;

        }

    }
}
