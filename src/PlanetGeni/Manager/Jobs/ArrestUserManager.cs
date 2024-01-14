using Common;
using DAO;
using DAO.Models;
using DTO.Db;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class ArrestUserManager
    {
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        private IPostCommentDTORepository postRepo = new PostCommentDTORepository();
        public ArrestUserManager()
        {

        }
        public void StartArresting(int runId)
        {
            RobberyDTORepository robberyRepo = new RobberyDTORepository();
            IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
            robberyRepo.ExecuteCrimeWatchWantedJob();
            IEnumerable<InJailDTO> userInJail =
            robberyRepo.GetAllUserInJail();
            if (userInJail != null)
            {
                ArrestUserDTO arrestedUser = new ArrestUserDTO();
                foreach (var item in userInJail)
                {
                    arrestedUser.UserId = item.UserId;
                    arrestedUser.NetWorth = bankRepo.GetNetWorth(item.UserId);
                    arrestedUser.ReducedNetWorth = arrestedUser.NetWorth * (1 - RulesSettings.RobberyAssetSeizePercent / 100);
                    robberyRepo.ExecuteArrestUser(arrestedUser);
                    AddUserNotification(item.UserId);
                    AddUserPost(item);
                    Console.WriteLine("userId {0} sent to Jail", item.UserId);
                }
            }
            else
            {
                Console.WriteLine("No One Sent to Jail");
            }
        }

        private void AddUserNotification(int userId)
        {

            StringBuilder parmtext = new StringBuilder();
            parmtext.AppendFormat("{0}|{1}",
                RulesSettings.RobberyMaxWantedLevel,
                RulesSettings.RobberyAssetSeizePercent);

            userNotif.AddNotification(false, string.Empty,
AppSettings.InJailNotificationId, parmtext.ToString(), 10, userId);
        }
        private void AddUserPost(InJailDTO jailTime)
        {
            StringBuilder parm = new StringBuilder();
            parm.AppendFormat("{0}",
                jailTime.TotalIncident
                    );

            Post post = new Post
            {
                Parms = parm.ToString(),
                PostContentTypeId = AppSettings.JailTimeContentTypeId,
                UserId = jailTime.UserId
            };
            postRepo.SavePost(post);
        }
    }
}
