using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Models._DB.AccountManagement;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AccessCode { get; set; }
    public DateTime? AccessCodeCreatedAt { get; set; }
}
