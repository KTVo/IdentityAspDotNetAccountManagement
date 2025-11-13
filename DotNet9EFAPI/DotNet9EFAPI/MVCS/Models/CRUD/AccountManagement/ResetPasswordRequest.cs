using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

public class ResetPasswordRequest: BaseRequest
{
    public string? SecurityCode { get; set; }
    public string? Password { get; set; }
}