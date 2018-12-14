namespace SendEmailClient.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SendEmailClient.Helpers;

    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        [HttpPost("sendEmail")]
        public bool SendEmail([FromBody] EmailDto email)
        {
            return EmailHelper.SendEmail(email);
        }
    }
}