using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Employment
    {
        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Business Business { get; set; }
    }
}
