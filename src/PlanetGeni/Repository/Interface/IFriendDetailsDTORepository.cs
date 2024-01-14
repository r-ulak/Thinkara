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
    public interface IFriendDetailsDTORepository
    {
        IEnumerable<UserEmailDTO> GetUserThatHasLowSocialAsset();
        bool IsFriend(int userId, int friendId);
        bool FollowFriend(FriendRelationDTO friendDTO);
        bool BlockFollower(FriendRelationDTO friendDTO);
        bool UnFollowFriend(FriendRelationDTO friendDTO);
        string GetFriendList(int userId);
        void UpdateEmailInvite(EmailInviteDTO inviteList);
        IEnumerable<ContactSourceDTO> GetContactSource(int userId);
        int FollowAllFriend(FollowAllDTO followFriends);
        IEnumerable<WebUserDTO> GetFriendSuggestion(FriendSuggestDTO suggestDTO);
        bool IgnoreSuggestion(FriendSuggestDTO friendDTO);


    }
}
