using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
    public class AdminBaseController : BaseController
    {
    }
}
