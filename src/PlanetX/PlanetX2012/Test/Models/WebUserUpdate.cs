using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class WebUserUpdate
    {
        public int UserId { get; set; }
        public string NameFirst { get; set; }
        public string NameMIddle { get; set; }
        public string NameLast { get; set; }
        public string EmailId { get; set; }
        public string OldNameFirst { get; set; }
        public string OldNameMIddle { get; set; }
        public string OldNameLast { get; set; }
        public string OldEmailId { get; set; }
        public string Picture { get; set; }
        public string ActionType { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
