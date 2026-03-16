using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchReviews;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryHandler : IRequestHandler<GetBranchReviewsQuery, ErrorOr<PaginatedResult<GetBranchReviewsQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetBranchReviewsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchReviewsQueryResponse>>> Handle(GetBranchReviewsQuery request, CancellationToken cancellationToken)
        {

            var curUserId = _currentUserService.UserId;

            var spec = new BranchReviewsPaginatedSpec(request.BranchId, request.Rating, curUserId, request.PageNumber, request.PageSize);
            var specForCount = new BranchReviewsPaginatedSpec(request.BranchId, request.Rating, null, request.PageNumber, request.PageSize);

            var reviews = await _unitOfWork.BranchReviews.ListAsync<GetBranchReviewsQueryResponse>(spec, cancellationToken);
            var totalCount = await _unitOfWork.BranchReviews.CountAsync(specForCount, cancellationToken);

            GetBranchReviewsQueryResponse? myReview = null;
            if (_currentUserService.IsAuthenticated)
            {
                myReview = await _unitOfWork.BranchReviews.GetAsync<GetBranchReviewsQueryResponse>(
                   r => r.BranchId == request.BranchId &&
                   r.UserId == curUserId!.Value, cancellationToken);
            }


            var result = new PaginatedResult<GetBranchReviewsQueryResponse>(reviews, totalCount, request.PageNumber, request.PageSize)
            {
                Meta = new BranchReviewsMeta(myReview)
            };

            return result;
        }
    }
}
