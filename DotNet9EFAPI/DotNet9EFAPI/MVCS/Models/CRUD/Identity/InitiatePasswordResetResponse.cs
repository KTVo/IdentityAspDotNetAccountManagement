using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

// WILL USE DECODED EMAIL FROM JWT AS DATABASE LOOKUP TO RESET PASSWORD
public class InitiatePasswordResetResponse : BaseResponse
{
    public string? PasswordResetToken { get; set; }
}