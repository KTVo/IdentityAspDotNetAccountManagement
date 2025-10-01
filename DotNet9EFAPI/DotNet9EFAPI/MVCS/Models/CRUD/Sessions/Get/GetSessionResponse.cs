using DotNet9EFAPI.MVCS.Models._base;
using DotNet9EFAPI.MVCS.Models._DB.Sessions.Get;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Sessions.Get;

public class GetSessionResponse : BaseResponse
{
    public Session? Session { get; set; }
}
