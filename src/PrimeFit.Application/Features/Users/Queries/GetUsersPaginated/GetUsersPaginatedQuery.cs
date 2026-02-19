using MediatR;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated {
    public class GetUsersPaginatedQuery : PaginatedQuery, IRequest<PaginatedResult<GetUsersPaginatedQueryResponse>> {

    }
}
