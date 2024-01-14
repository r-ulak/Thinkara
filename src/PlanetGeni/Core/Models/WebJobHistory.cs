using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class WebJobHistory
    {
        public sbyte JobId { get; set; }
        public int RunId { get; set; }
        public System.DateTime CreatedAT { get; set; }
    }
}
