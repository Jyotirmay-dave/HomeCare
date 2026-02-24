using System;
using System.ComponentModel.DataAnnotations;

namespace HomeCare_dotnet.DTOs;

public class UserLoginDTO
{
    public required string UserName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
}
