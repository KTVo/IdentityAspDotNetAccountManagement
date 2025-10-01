using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.JWT;

public class TokenResponse : BaseResponse
{
    public string? Token { get; set; }
}
