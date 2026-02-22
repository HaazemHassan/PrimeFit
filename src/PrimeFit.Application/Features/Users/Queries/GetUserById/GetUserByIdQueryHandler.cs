using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Specifications.Shared;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<GetUserByIdQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetByIdSpec<DomainUser>(request.OwnerUserId);
            var user = await _unitOfWork.Users.GetAsync<GetUserByIdQueryResponse>(spec, cancellationToken);
            if (user is null)
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found");

            return user;
        }
    }
}
