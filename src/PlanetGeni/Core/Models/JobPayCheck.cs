using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class JobPayCheck
    {
        public System.Guid TaskId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public System.DateTime PayDate { get; set; }
    }
}
