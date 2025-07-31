using BlogSystem.Api.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystem.Api.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return Ok(new ApiErrorResponse(code));
        }
    }
}
