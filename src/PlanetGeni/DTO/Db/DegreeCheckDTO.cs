using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Db
{
    public class DegreeCheckDTO
    {
        public sbyte MajorId { get; set; }
        public sbyte DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string MajorName { get; set; }
        public string ImageFont { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string DegreeImageFont { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
    }
}
