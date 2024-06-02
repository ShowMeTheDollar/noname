using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenRoom.Web.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        public readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Send(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok(new { status = "Success" });
        }
    }
}
