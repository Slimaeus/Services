﻿namespace Services.WebApi.Services;

using global::Services.WebApi.Configurations;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService(IOptions<GmailSettings> options)
    {
        _smtpHost = options.Value.Host;
        _smtpPort = options.Value.Port;
        _smtpUsername = options.Value.UserName;
        _smtpPassword = options.Value.Password;
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

