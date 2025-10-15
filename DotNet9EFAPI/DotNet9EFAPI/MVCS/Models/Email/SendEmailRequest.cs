using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.Email;

public class SendEmailRequest : BaseRequest
{
    public string? ToEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}