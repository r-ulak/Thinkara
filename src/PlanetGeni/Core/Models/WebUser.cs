using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class WebUser
    {
        public int UserId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string EmailId { get; set; }
        public string Picture { get; set; }
        public sbyte Active { get; set; }
        public Nullable<sbyte> OnlineStatus { get; set; }
        public string CountryId { get; set; }
        public int UserLevel { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}