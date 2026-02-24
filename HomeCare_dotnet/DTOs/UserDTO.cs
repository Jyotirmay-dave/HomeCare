using System;

namespace HomeCare_dotnet.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
}
