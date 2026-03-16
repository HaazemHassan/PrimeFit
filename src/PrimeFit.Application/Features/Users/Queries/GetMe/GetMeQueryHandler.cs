using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Users.Queries.GetMe
{
    public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<GetMeQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetMeQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<GetMeQueryResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Error.Unauthorized(code: ErrorCodes.Authentication.InvalidAccessToken, description: "Invalid access token");
            }

            var user = await _unitOfWork.Users.GetAsync(u => u.Id == userId.Value, cancellationToken);
            if (user is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found");
            }

            var branches = await _unitOfWork.Branches.ListAsync<BranchLiteDto>(b => b.OwnerId == user.Id, cancellationToken);
            var roles = _currentUserService.GetRoles();
            UserRole? userRole = roles.Count > 0 ? roles[0] : null;

            return new GetMeQueryResponse
            {
                Id = userId.Value,
                UserType = user.UserType,
                UserRole = userRole,
                Branches = branches
            };
        }
    }
}
