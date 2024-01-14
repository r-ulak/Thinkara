using Dao.Models;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IWebUserDTORepository
    {
        void GiveCreditForAcceptedInvitationandFollowInvitee(WebUserContact webcontact);
        WebUserContact GetInvitationSender(string invitationId, string emailId);
        string GetFirstName(int userId);
        string GetEmailByUserId(int userId);
        bool IsThisFirstLogin(int userId);
        void TrackUserActivity(UserActivityLog activity);
        string isLeader(int userId);
        WebUserIndexDTO GetWebUserIndexDTO(int userId);
        bool UpdateProfileName(WebUserInfoDTO webInfo);
        bool UpdateProfilePic(WebUserInfoDTO webInfo);
        IEnumerable<WebUserDTO> GetWebUserList(int[] userId);
        bool IsAllowedUsers(string emailId);
        void ApplyCreidtScore();
        void UserChangingLevel(short notificationId, sbyte postConetentId);
        void SaveWebUserContacts(InitializeWebUser webInfo);
        int InitializeUser(InitializeWebUser webInfo);
        UserProfileDTO GetProfileStat(int userId);
        string GetUserPicture(int userId);
        Dictionary<string, string> GetWebUserDictionary(int userId);
        WebUser GetWebUser(int userId);
        string GetFullName(int userId);
        string GetCountryId(int userId);
        WebUserDTO GetUserPicFName(int userId);
        IEnumerable<WebUser> GetRandomWebUsers(int number, string countryCode);
        int GetUserIdByEmail(string emailId);
        WebUserInfo GetWebUserInfo(int userId, string providerInfo);
        void ClearCacheProfileUpdate(int userId);


    }
}
