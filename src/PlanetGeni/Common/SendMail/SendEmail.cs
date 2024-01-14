using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Common.SendMail
{
    public class SendEmail
    {
        public void SendGridasync(EmailMessage message)
        {
            var myMessage = new MailMessage();
            myMessage.To.Add(message.Destination);
            myMessage.Subject = message.Subject;

            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html));

            SmtpClient smtp = new SmtpClient();
            smtp.Send(myMessage);

        }
    }
}
