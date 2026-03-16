using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    public class AddBranchReviewCommandHandler : IRequestHandler<AddBranchReviewCommand, ErrorOr<AddBranchReviewCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddBranchReviewCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AddBranchReviewCommandResponse>> Handle(AddBranchReviewCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var hasSubscription = await _unitOfWork.Subscriptions.AnyAsync(
                sub => sub.UserId == userId && sub.BranchId == request.BranchId,
                cancellationToken);

            if (!hasSubscription)
            {
                return Error.Forbidden(
                    ErrorCodes.BranchReview.NotSubscribed,
                    "You must have been subscribed to this branch to leave a review.");
            }

            var alreadyReviewed = await _unitOfWork.BranchReviews.AnyAsync(
                r => r.UserId == userId && r.BranchId == request.BranchId,
                cancellationToken);

            if (alreadyReviewed)
            {
                return Error.Conflict(
                    ErrorCodes.BranchReview.AlreadyReviewed,
                    "You have already submitted a review for this branch.");
            }

            var reviewResult = BranchReview.CreateBranchReview(request.BranchId, userId, request.Rating, request.Comment);

            if (reviewResult.IsError)
            {
                return reviewResult.Errors;
            }

            await _unitOfWork.BranchReviews.AddAsync(reviewResult.Value, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var review = reviewResult.Value;

            var response = _mapper.Map<AddBranchReviewCommandResponse>(review);
            return response;
        }
    }
}
