using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Education
    {
        public int UserId { get; set; }
        public short MajorId { get; set; }
        public sbyte DegreeId { get; set; }
        public string Status { get; set; }
        public decimal CompletionCost { get; set; }
        public System.DateTime ExpectedCompletion { get; set; }
        public System.DateTime NextBoostAt { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
