using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.Extensions.Options;
using DotNet9EFAPI.Helpers.Email;

namespace DotNet9EFAPI.MVCS.Services.Email;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly EmailSettings _emailSettings;

    public SmtpEmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
    }

    /// <summary>
    /// SENDS AN EMAIL TO OUR TESTING EMAIL ACCOUNT VIA SMTP
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<SendEmailResponse> SendEmailAsync()
        {
            const string subject = "Test Email From New Web API!"; 
            const string body = "<h1>This is a test email sent from the new .NET 9 Web API!</h1><p>If you received this email, the functionality works correctly.</p>"; 
            
            try 
            { 
                // SET EMAIL REQUEST PROPERTIES
                SendEmailRequest sendEmailRequest = new()
                {
                    Body = body,
                    Subject = subject,
                    FromEmail = _emailSettings.HostEmail ??
                                throw new ArgumentNullException(nameof(_emailSettings.HostEmail)),
                    Password = _emailSettings.HostEmailPassword ??
                               throw new ArgumentNullException(nameof(_emailSettings.HostEmailPassword)),
                    ToEmail = _emailSettings.TestToEmail ??
                              throw new ArgumentNullException(nameof(_emailSettings.TestToEmail)),
                    SmtpServer = _emailSettings.HostSmtpServer ??
                                 throw new ArgumentNullException(nameof(_emailSettings.HostSmtpServer)),
                    SmtpPort = _emailSettings.HostSmtpServerPort
                };
                
                // SENDS AN EMAIL SMTP
                bool emailSentSuccessfully = await SendEmailHelper.SendEmailAsync(sendEmailRequest);

                if (emailSentSuccessfully == false)
                {
                    return new()
                    {
                        IsSuccessful = false,
                        Message = AppMessages.EmailSentFailed
                    };
                }

                return new()
                {
                    IsSuccessful = true,
                    Message = AppMessages.EmailSentSuccessfully
                };
            } 
            catch (Exception ex) 
            { 
                return new()
                {
                    IsSuccessful = false,
                    Message = ex.Message
                };
            }
        }
}