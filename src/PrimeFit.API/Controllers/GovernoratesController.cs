using Microsoft.AspNetCore.Mvc;
using PrimeFit.Application.Features.Governorates.Queries.GetGovernorates;

namespace PrimeFit.API.Controllers
{
    public class GovernoratesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetGovernorates()
        {
            var result = await Mediator.Send(new GetGovernoratesQuery());
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
