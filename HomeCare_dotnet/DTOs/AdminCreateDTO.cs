using System;
using System.ComponentModel.DataAnnotations;

namespace HomeCare_dotnet.DTOs;

public class AdminCreateDTO
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string UserName { get; set; }
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$",
        ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one number, and one special character.")]
    public required string Password { get; set; }
}
