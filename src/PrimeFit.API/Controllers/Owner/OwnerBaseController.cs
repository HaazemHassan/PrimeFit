using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Controllers.Owner
{
    [Route("api/Owner/[controller]")]
    [Authorize(Roles = $"{nameof(UserRole.Owner)}")]
    public class OwnerBaseController : BaseController
    {
    }
}
