using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace VerificationEmailSender.SendGrid
{
    public interface ISendgrindEmailSender
    {
        Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details, string SindGridApiKey);
    }
    public class SendgrindEmailSender : ISendgrindEmailSender
    {
        public SendgrindEmailSender()
        {
        }
        public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details, string SindGridApiKey)
        {
            // Get the SendGrid key

            var apiKey = SindGridApiKey;

            // Create a new SendGrid client
            var client = new SendGridClient(apiKey);

            // From
            var from = new EmailAddress(details.FromEmail, details.FromName);

            // To
            var to = new EmailAddress(details.ToEmail, details.ToName);

            // Subject
            var subject = details.Subject;

            // Create Email class ready to send
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                // Plain content
                details.IsHTML ? null : details.Content,
                // HTML content
                details.IsHTML ? details.Content : null);

            // Finally, send the email...
            var response = await client.SendEmailAsync(msg);

            // If we succeeded...
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                // Return successful response
                SendEmailResponse emailResponse = new SendEmailResponse();
                emailResponse.SendGridResponse = response;
                return emailResponse;
            }

            // Otherwise, it failed...

            try
            {
                // Get the result in the body
                var bodyResult = await response.Body.ReadAsStringAsync();

                // Deserialize the response
                var sendGridResponse = JsonConvert.DeserializeObject<SendGrindResponse>(bodyResult);
                // Add any errors to the response
                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
                };

                // Make sure we have at least one error
                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    // Add an unknown error
                    // TODO: Localization
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact support." });

                // Return the response
                errorResponse.SendGridResponse = response;
                return errorResponse;

            }
            catch (Exception ex)
            {
                // TODO: Localization
                // Break if we are debugging
                if (Debugger.IsAttached)
                {
                    var error = ex;
                    Debugger.Break();
                }

                // If something unexpected happened, return message
                return new SendEmailResponse
                {
                    Errors = new List<string>(new[] { "Unknown error occurred" })
                };
            }
        }

    }//Class
}
