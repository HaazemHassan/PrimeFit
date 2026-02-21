using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;

namespace PrimeFit.API.Controllers
{
    public class BranchesController : BaseController
    {
        [HttpGet("{Id:int}", Name = RouteNames.Branches.GetBranchById)]
        public async Task<IActionResult> GetBranchById([FromRoute] int Id)
        {
            return NoContent();
        }

    }
}
