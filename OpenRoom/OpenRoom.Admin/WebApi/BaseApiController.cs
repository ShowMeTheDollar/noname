using OpenRoom.Admin.Filters;
using Microsoft.AspNetCore.Mvc;

namespace OpenRoom.Admin.WebApi
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [ApiExceptionFilter]
    public class BaseApiController : ControllerBase
    {

    }
}
