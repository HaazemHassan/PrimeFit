using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.Employees;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    public class GetBranchEmployeesQueryHandler : IRequestHandler<GetBranchEmployeesQuery, ErrorOr<PaginatedResult<GetBranchEmployeesQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public GetBranchEmployeesQueryHandler(IUnitOfWork unitOfWork, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchEmployeesQueryResponse>>> Handle(GetBranchEmployeesQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.EmployeesView, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var dataSpec = new BranchEmployeesPaginatedSpec(request.BranchId, request.PageNumber, request.PageSize);

            var employees = await _unitOfWork.Employees.ListAsync<GetBranchEmployeesQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Employees.CountAsync(dataSpec, cancellationToken);


            return new PaginatedResult<GetBranchEmployeesQueryResponse>(employees, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
