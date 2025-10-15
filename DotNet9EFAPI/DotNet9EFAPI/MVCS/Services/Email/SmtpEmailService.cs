using System.Net;
using System.Net.Mail;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.Extensions.Options;

namespace DotNet9EFAPI.MVCS.Services.Email;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly EmailSettings _emailSettings;

    // NOTE IOptionsMonitor WILL NOTICE CHANGES IN APPSETTINGS.JSON WITHOUT RESTARTING THE APP
    public SmtpEmailService(IOptionsMonitor<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.CurrentValue ?? throw new ArgumentNullException(nameof(emailSettings));
    }

    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest model)
    {
        try
        {
            if (model.JWTToken == null) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; };
            // TODO: VERIFY JWT TOKEN HERE
            
            // NULL CHECKS
            if (string.IsNullOrEmpty(model.ToEmail) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; };
            if (string.IsNullOrEmpty(model.Subject) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; };
            if (string.IsNullOrEmpty(model.Body) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; };
            
            // IMPORT SETTINGS FOR APPSETTINGS VIA MODEL CLASS EmailSettings
            EmailSettings? smtpSection = _emailSettings;
            
            // NULL CHECKS
            if (string.IsNullOrEmpty(smtpSection.HostEmail) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            if (string.IsNullOrEmpty(smtpSection.HostEmailPassword) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            if (string.IsNullOrEmpty(smtpSection.HostSmtpServer) == true) { return new SendEmailResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            
            // CREATES CLIENT TO SEND EMAIL
            using var client = new SmtpClient(smtpSection.HostSmtpServer, smtpSection.HostSmtpServerPort)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpSection.HostEmail, smtpSection.HostEmailPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mail = new MailMessage(smtpSection.HostEmail, to: model.ToEmail, model.Subject, model.Body);
            
            // SENDS EMAIL
            await client.SendMailAsync(mail);
            
            return new SendEmailResponse { IsSuccessful = true, Message = AppMessages.EmailSuccess };
        }
        catch (Exception ex)
        {
            return new SendEmailResponse { IsSuccessful = false, Message = ex.Message };
        }
    }
}