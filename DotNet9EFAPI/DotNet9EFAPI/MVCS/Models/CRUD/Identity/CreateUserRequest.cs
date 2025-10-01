namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class CreateUserRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TwoFactorCode { get; set; }
}
