using System.Net;
using System.Net.Mail;
using DotNet9EFAPI.MVCS.Models.Email;

namespace DotNet9EFAPI.Helpers.Email;

public static class SendEmailHelper
{
    /// <summary>
    /// HELPER METHOD FOR SENDING EMAIL VIA SMTP
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static async Task<bool> SendEmailAsync(SendEmailRequest model)
    {
        if (string.IsNullOrEmpty(model.Body)) { return false; }
        if (string.IsNullOrEmpty(model.Subject)) { return false; }
        if (string.IsNullOrEmpty(model.FromEmail)) { return false; }
        if (string.IsNullOrEmpty(model.Password)) { return false; }
        if (string.IsNullOrEmpty(model.SmtpServer)) { return false; }
        if (string.IsNullOrEmpty(model.Subject)) { return false; }
        if (string.IsNullOrEmpty(model.ToEmail)) { return false; }
        
        using (SmtpClient client = new(model.SmtpServer, model.SmtpPort)) 
        { 
            client.Credentials = new NetworkCredential(model.FromEmail, model.Password); client.EnableSsl = true; 
            // Outlook requires SSL
            var mailMessage = new MailMessage { 
                From = new MailAddress(model.FromEmail), 
                Subject = model.Subject, 
                Body = model.Body, 
                IsBodyHtml = true 
                // Set to false for plain text
            }; 
            mailMessage.To.Add(model.ToEmail); 
            await client.SendMailAsync(mailMessage);
        }

        return true;
    }
}