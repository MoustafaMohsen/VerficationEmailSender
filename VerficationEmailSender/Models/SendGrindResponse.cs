using SendGrid;
using System.Collections.Generic;

namespace VerficationEmailSender.SendGrid
{
    public class SendGrindResponse
    {
        public List<SendGridResponseError> Errors { get; set; }
    }

    public class SendEmailResponse
    {
        public List<string> Errors { get; set; }
        public bool isSuccesful => Errors == null ? true : false;
        public Response SendGridResponse { get; set; }
    }
}
