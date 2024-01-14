using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class my_aspnet_sessioncleanup
    {
        public System.DateTime LastRun { get; set; }
        public int IntervalMinutes { get; set; }
        public int ApplicationId { get; set; }
    }
}
