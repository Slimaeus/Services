using Microsoft.AspNetCore.Mvc;
using Services.WebApi.Services;

namespace Services.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    public EmailService EmailService => HttpContext.RequestServices.GetRequiredService<EmailService>();

    [HttpPost]
    public async Task<IActionResult> SendMessage()
    {
        await EmailService.SendEmailAsync("example@gmail.com", "Nice", "This is message");
        return Ok();
    }

}
