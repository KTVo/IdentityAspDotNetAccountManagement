using DotNet9EFAPI.MVCS.Models.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DotNet9EFAPI.MVCS.Services.Email;

public class SendGridService : ISendGridService
{
    private readonly IConfiguration _configuration;
    private readonly string apiKey;

    public SendGridService(IConfiguration configuration)
    {
        _configuration =  configuration ?? throw new ArgumentNullException(nameof(configuration));
        apiKey = _configuration["Email:SendGrid:ApiKey"] ?? throw new ArgumentNullException("Email:SendGrid:ApiKey is required.");
    }

    public async Task<SendEmailResponse> SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress(_configuration["Email:HostEmail"], _configuration["Email:HostName"]);
            EmailAddress to = new EmailAddress(toEmail);
            string htmlContent = $"<strong>{message}</strong>";
            SendGridMessage msg = MailHelper.CreateSingleEmail(
                from, 
                to, 
                subject, 
                message, 
                htmlContent
                );
            
            Response sendEmailResponse = await client.SendEmailAsync(msg);

            SendEmailResponse response = new SendEmailResponse()
            {
                Message = sendEmailResponse.ToString(),
                IsSuccessful = sendEmailResponse.IsSuccessStatusCode
            };

            return response;
        }
        catch (Exception e)
        {
            SendEmailResponse response = new SendEmailResponse()
            {
                Message = e.Message,
                IsSuccessful = false
            };
            
            return response;
        }
    }
}