using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class ChangeEmailRequest : BaseRequest
{
    public string? NewEmail { get; set; }
}