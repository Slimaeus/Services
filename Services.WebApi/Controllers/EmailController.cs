using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.WebApi.Configurations;
using Services.WebApi.Services;

namespace Services.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    public EmailService EmailService => HttpContext.RequestServices.GetRequiredService<EmailService>();
    public IOptions<GmailSettings> Options => HttpContext.RequestServices.GetRequiredService<IOptions<GmailSettings>>();
    [HttpGet]
    public GmailSettings GetConfig()
    {
        return Options.Value;
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage()
    {
        await EmailService.SendEmailAsync("example@gmail.com", "Nice", "This is message");
        return Ok();
    }

}
