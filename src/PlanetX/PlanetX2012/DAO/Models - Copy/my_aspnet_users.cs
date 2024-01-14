using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class my_aspnet_users
    {
        public int id { get; set; }
        public int applicationId { get; set; }
        public string name { get; set; }
        public bool isAnonymous { get; set; }
        public Nullable<System.DateTime> lastActivityDate { get; set; }
    }
}
