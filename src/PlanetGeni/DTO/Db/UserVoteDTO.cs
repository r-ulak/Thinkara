
using Dao.Models;
using System.Collections.Generic;
namespace DTO.Db
{
    public class UserVoteDTO
    {
        public int InitiatorUserId { get; set; }
        public int TaskTypeId { get; set; }
        public sbyte Active { get; set; }
        public string FullName { get; set; }
        public sbyte OnlineStatus { get; set; }
        public string LogoPicture { get; set; }
        public string UserPicture { get; set; }
        public string ShortDescription { get; set; }
        public string VoteJsAction { get; set; }
        public string Description { get; set; }
        public string Parms { get; set; }
        public sbyte ChoiceType { get; set; }
        public sbyte MaxChoiceCount { get; set; }
        public IEnumerable<UserVoteChoice> Choices { get; set; }
    }
}

