using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class ChangeUserRequest : BaseRequest
{
    public bool UpdateUserName { get; set; } = false;
    public string? NewUserName { get; set; }
}