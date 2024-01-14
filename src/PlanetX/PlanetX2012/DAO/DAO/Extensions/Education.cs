using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DAO.Models
{
    public partial class Education
    {
        [NotMapped]
        public string Degree { get; set; }
        [NotMapped]
        public string Major { get; set; }
        [NotMapped]
        public string UniversityName { get; set; }
        
    }
}
