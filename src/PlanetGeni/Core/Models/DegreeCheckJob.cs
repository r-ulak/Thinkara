using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class DegreeCheckJob
    {
        public int RunId { get; set; }
        public int UserId { get; set; }
        public short MajorId { get; set; }
        public sbyte DegreeId { get; set; }
    }
}
