using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserVoteSelectedChoice
    {
        public Guid TaskId { get; set; }
        public int ChoiceId { get; set; }
        public int UserId { get; set; }
        public sbyte Score { get; set; }
    }
}
         