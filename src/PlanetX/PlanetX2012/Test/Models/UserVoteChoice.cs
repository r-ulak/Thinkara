using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserVoteChoice
    {
        public int ChoiceId { get; set; }
        public int TaskTypeId { get; set; }
        public string ChoiceText { get; set; }
        public string ChoiceLogo { get; set; }
    }
}
