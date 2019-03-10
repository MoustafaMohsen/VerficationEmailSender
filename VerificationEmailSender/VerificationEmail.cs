using System.Threading.Tasks;
using VerificationEmailSender.Models;
using VerificationEmailSender.SendGrid;

namespace VerificationEmailSender
{
    public interface IVerificationEmail
    {
        Task<SendEmailResponse> SendUserVerificationEmailAsync(EmailSettings emailSettings);
    }
    public class VerificationEmail : IVerificationEmail
    {
        protected ISendgrindEmailSender emailSender;
        public VerificationEmail( ISendgrindEmailSender sender)
        {
            emailSender = sender;
        }
        public async Task<SendEmailResponse> SendUserVerificationEmailAsync( EmailSettings emailSettings)
        {
            // Getting email variables
            //var templateText = VerificationTemplateString.TemplateString;
            var templateText = emailSettings.Template;
            // Replace template placeholders
            var EmailContent = templateText.Replace("--Title--", emailSettings.Title)
                                        .Replace("--Content1--", emailSettings.Content1)
                                        .Replace("--Content2--", emailSettings.Content2)
                                        .Replace("--ButtonText--", emailSettings.ButtonText)
                                        .Replace("--ButtonUrl--", emailSettings.ButtonUrl);

            // Initiat email details
            var EmailDetails = new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = emailSettings.FromEmail,
                FromName = emailSettings.FromName,
                ToEmail = emailSettings.ToEmail,
                ToName = emailSettings.ToName,
                Subject = emailSettings.Subject,
                Content = EmailContent
            };

            // Send email
            SendEmailResponse sendEmailResponse = await emailSender.SendEmailAsync(EmailDetails, emailSettings.SendGridApiKey);
            return sendEmailResponse;
        }

    }
}
