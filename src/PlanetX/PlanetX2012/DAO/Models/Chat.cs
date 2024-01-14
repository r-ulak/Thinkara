using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Chat
    {
        public int ChatId { get; set; }
        public Nullable<int> UserId { get; set; }
        public int To { get; set; }
        public string Msg { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
