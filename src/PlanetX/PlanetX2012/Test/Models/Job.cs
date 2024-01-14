using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Job
    {
        public short JobId { get; set; }
        public short JobTypeId { get; set; }
        public short JobCodeId { get; set; }
        public short CompanyId { get; set; }
        public Nullable<System.DateTime> ExipiryDate { get; set; }
        public decimal Salary { get; set; }
        public string Frequency { get; set; }
        public sbyte HrsPerWeek { get; set; }
    }
}
