using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.Email;

namespace DotNet9EFAPI.MVCS.Services.Email;

public interface ISmtpEmailService
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest model);
}