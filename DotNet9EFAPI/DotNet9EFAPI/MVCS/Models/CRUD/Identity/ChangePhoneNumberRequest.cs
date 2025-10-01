using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class ChangePhoneNumberRequest : BaseRequest
{
    public string? NewPhoneNumber { get; set; }
}