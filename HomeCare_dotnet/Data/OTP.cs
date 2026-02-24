using System;

namespace HomeCare_dotnet.Data;

public class OTP
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string OtpCode { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
