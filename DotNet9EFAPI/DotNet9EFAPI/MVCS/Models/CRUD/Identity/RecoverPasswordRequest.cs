using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class RecoverPasswordRequest: BaseRequest
{
    public string? Passcode { get; set; }
}