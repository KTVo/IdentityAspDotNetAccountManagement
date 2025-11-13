using DotNet9EFAPI.MVCS.Models._base;

namespace DotNet9EFAPI.MVCS.Models.Email;

public class SendEmailRequest
{
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string? ToEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? FromEmail { get; set; }
    public string? Password { get; set; }
}