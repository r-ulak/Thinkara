using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class LoanCode
    {
        public LoanCode()
        {
            this.LoanFromBusinesses = new List<LoanFromBusiness>();
            this.LoanFromPersons = new List<LoanFromPerson>();
        }

        public sbyte LoanType { get; set; }
        public string Code { get; set; }
        public virtual ICollection<LoanFromBusiness> LoanFromBusinesses { get; set; }
        public virtual ICollection<LoanFromPerson> LoanFromPersons { get; set; }
    }
}
