using DotNet9EFAPI.MVCS.Models._base;
using DotNet9EFAPI.MVCS.Models._DB.Identity;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Identity;

public class CreateUserRequest : BaseRequest
{
    public User? User { get; set; }
}
