namespace Services.WebApi.Services;

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService()
    {
        _smtpHost = "smtp.gmail.com";
        _smtpPort = 587;
        _smtpUsername = "mail";
        _smtpPassword = "password";
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
        {
            Host = _smtpHost,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpUsername),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(recipientEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}

