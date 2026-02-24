using System;
using System.ComponentModel.DataAnnotations;

namespace HomeCare_dotnet.DTOs;

public class OtpVerifyDTO
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string OtpCode { get; set; }
}
