using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobTestTask.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        [NonAction]
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            if (result.IsSuccess && result.Value == null)
            {
                return NotFound();
            }
            if (result.IsSuccess && result.Value != null)
            {
                return Ok(result.Value);
            }

            return BadRequest();
        }
    }
}
