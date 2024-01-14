using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UniversityCode
    {
        public short UniversityId { get; set; }
        public string Name { get; set; }
        public virtual UniversityLocation UniversityLocation { get; set; }
    }
}
