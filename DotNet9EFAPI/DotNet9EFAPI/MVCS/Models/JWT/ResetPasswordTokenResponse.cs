using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.JWT;

public class ResetPasswordTokenResponse : BaseResponse
{
    public string? UserToken { get; set; }
    public string? PassCode { get; set; }
}