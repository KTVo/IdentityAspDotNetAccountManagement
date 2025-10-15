using DotNet9EFAPI.MVCS.Models.Email;

namespace DotNet9EFAPI.MVCS.Services.Email;

public interface ISendGridService
{
    Task<SendEmailResponse> SendEmailAsync(string toEmail, string subject, string message);
}