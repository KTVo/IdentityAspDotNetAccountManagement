using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

public sealed class ResetPasswordRequest: BaseRequest
{
    public string? AccessCode { get; set; }
    public string? NewPassword { get; set; }
}