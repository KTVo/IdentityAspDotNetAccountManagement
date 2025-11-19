using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

public class RecoverPasswordRequest: BaseRequest
{
    public string? Email { get; set; }
    public string? Username { get; set; }
}