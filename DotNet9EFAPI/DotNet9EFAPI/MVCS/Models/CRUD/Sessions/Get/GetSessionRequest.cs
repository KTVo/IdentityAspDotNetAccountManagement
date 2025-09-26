using DotNet9EFAPI.MVCS.Models._base;
using DotNet9EFAPI.MVCS.Models._DB.Sessions.Get;

namespace DotNet9EFAPI.MVCS.Models.CRUD.Sessions.Get;

public class GetSessionRequest : BaseRequest
{
    public Session? Session { get; set; }
}