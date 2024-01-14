using System;

namespace Dao.Models
{
    public partial class UserActivityLog
    {
        public int UserId { get; set; }
        public int Hit { get; set; }
        public string IPAddress { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime FirstLogin { get; set; }
    }
}
