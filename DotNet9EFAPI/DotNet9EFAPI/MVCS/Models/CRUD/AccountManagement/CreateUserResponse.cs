using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

public class CreateUserResponse : BaseResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}
