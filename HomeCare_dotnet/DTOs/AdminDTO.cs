using System;
using System.ComponentModel.DataAnnotations;

namespace HomeCare_dotnet.DTOs;

public class AdminDTO
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public bool IsActive { get; set; }
}
