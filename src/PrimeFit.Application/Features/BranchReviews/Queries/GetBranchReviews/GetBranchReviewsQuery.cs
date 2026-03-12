using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetBranchReviewsQueryResponse>>>
    {
        public int BranchId { get; set; }
        public int? Rating { get; set; }
    }
}
