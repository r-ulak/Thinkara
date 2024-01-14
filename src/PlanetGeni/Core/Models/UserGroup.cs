using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserGroup
    {
        public int UserGroupId { get; set; }
        public string UserGroupName { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Description { get; set; }
        public int UserGroupType { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
