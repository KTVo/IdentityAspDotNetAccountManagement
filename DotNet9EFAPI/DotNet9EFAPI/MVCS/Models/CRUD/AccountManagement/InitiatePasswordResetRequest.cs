using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

// WILL USE DECODED EMAIL FROM JWT AS DATABASE LOOKUP TO RESET PASSWORD
public class InitiatePasswordResetRequest : BaseRequest
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}