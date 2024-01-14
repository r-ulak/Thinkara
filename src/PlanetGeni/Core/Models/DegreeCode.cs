using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class DegreeCode
    {
        public sbyte DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string DegreeImageFont { get; set; }
        public sbyte DegreeRank { get; set; }
    }
}
