using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class MajorCode
    {
        public short MajorId { get; set; }
        public string MajorName { get; set; }
        public string ImageFont { get; set; }
        public string Description { get; set; }
        public sbyte MajorRank { get; set; }
        public decimal Cost { get; set; }
        public sbyte Duration { get; set; }
        public decimal JobProbability { get; set; }
    }
}
