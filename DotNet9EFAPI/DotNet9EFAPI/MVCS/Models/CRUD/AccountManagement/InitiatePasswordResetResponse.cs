using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

// WILL USE DECODED EMAIL FROM JWT AS DATABASE LOOKUP TO RESET PASSWORD
public class InitiatePasswordResetResponse : BaseResponse
{
    // ACCESS CODE TO RESET PASSWOD
    public string? AccessCode { get; set; }
    public string? JWTToken { get; set; }
}