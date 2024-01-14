using System;
using System.Collections.Generic;

namespace Dao.Models
{
    public partial class UserVoteChoice
    {
        public int TaskTypeId { get; set; }
        public int ChoiceId { get; set; }
        public string ChoiceText { get; set; }
        public string ChoiceLogo { get; set; }
    }
}
