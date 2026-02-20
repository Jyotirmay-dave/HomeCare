using System;

namespace HomeCare_dotnet.Data;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsServiceProvider { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
