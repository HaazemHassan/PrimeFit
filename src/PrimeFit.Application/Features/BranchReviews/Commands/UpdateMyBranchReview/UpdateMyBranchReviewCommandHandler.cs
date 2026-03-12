using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    public class UpdateMyBranchReviewCommandHandler : IRequestHandler<UpdateMyBranchReviewCommand, ErrorOr<UpdateMyBranchReviewCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMyBranchReviewCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdateMyBranchReviewCommandResponse>> Handle(UpdateMyBranchReviewCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var review = await _unitOfWork.BranchReviews.GetAsync(
                r => r.Id == request.ReviewId && r.UserId == userId,
                cancellationToken);

            if (review is null)
            {
                return Error.NotFound(
                    ErrorCodes.BranchReview.NotFound,
                    "Review not found.");
            }


            var updateResult = review.Update(request.Rating, request.Comment);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UpdateMyBranchReviewCommandResponse>(review);
        }
    }
}
