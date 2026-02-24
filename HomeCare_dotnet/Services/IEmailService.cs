using System;

namespace HomeCare_dotnet.Services;

public interface IEmailService
{
    Task SendOtpAsync(string toEmail, string otp);
}
