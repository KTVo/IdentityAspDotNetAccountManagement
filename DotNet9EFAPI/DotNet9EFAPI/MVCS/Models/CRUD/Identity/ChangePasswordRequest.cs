using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class ChangePasswordRequest : BaseRequest
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}