using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Models._DB.Identity;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
}
