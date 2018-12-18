using System;
using System.Collections.Generic;
using System.Text;

namespace VerficationEmailSender.Models
{
    public class EmailSettings
    {
        public string SendGridApiKey { get; set; }
        public string Title { get; set; } = "Verify Email";
        public string Subject { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }

        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }

        public string FromName { get; set; }
        public string FromEmail { get; set; }

        public string ToName { get; set; }
        public string ToEmail { get; set; }


        public string Template { get; set; }
    }
}
