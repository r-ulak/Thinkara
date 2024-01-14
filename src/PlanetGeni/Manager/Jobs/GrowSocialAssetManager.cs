using Common;
using Common.SendMail;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class GrowSocialAssetManager
    {
        IFriendDetailsDTORepository friendRepo = new FriendDetailsDTORepository();
        public GrowSocialAssetManager()
        {

        }
        public void SendEmailNotification(int runId)
        {

            if (!AppSettings.SendEmailNotfication)
            {
                Console.WriteLine("SendEmailNotfication currently is {0}", AppSettings.SendEmailNotfication);
                return;
            }
            Console.WriteLine("getting the GetUserThatHasLowSocialAsset... ");
            IEnumerable<UserEmailDTO> userEmail = friendRepo.GetUserThatHasLowSocialAsset();
            SendEmail mailservice = new SendEmail();
            EmailMessage message = new EmailMessage();
            StringBuilder emailBody = new StringBuilder();
            message.Subject = "Grow Your Social Assets";
            Console.WriteLine("got {0} NewNotificationByUser... ", userEmail.Count());
            foreach (var item in userEmail)
            {
                emailBody.Clear();
                emailBody.Append(AppSettings.OfflineNotficationEmailtemplate);
                emailBody.Replace(":FirstName", item.NameFirst);
                emailBody.Replace(":Message", string.Format("Time to grow your social assets, you can grow your social assets by inviting more of your friends. Import your email contacts, send invite and make some more cash. You can import your popular email services like Microsoft (Hotmail, live, outlook), Gmail and Yahoo. You can find the social section on Navigation bar top right."));

                message.Destination = item.EmailId;
                message.Body = emailBody.ToString();

                 mailservice.SendGridasync(message);
                Console.WriteLine("Emailing {0}", message.Destination);
            }
            Console.WriteLine("calling UpdateEmailSentByTime...");

            Console.WriteLine("Finished SendEmailNotification, total of {0}", userEmail.Count());

        }
    }
}
