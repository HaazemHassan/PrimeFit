using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Contracts.Infrastructure
{
    public interface IApplicationUserService
    {
        public Task<ErrorOr<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default);


    }

}