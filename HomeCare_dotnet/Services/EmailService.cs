using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace HomeCare_dotnet.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration configuration)
    {
        _config = configuration;
    }

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("HomeCare", _config["Email:From"]));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Your OTP CODE";
        message.Body = new TextPart("html")
        {
            Text = $"<h3>Your OTP is: <strong>{otp}</strong></h3><p>Valid for 5 minutes.</p>"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Email:Host"], int.Parse(_config["Email:Port"]), false);
        await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
