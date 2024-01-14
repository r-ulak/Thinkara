using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Business
    {
        public Business()
        {
            this.LoanFromBusinesses = new List<LoanFromBusiness>();
        }

        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public string BusinessName { get; set; }
        public short BusinessTypeId { get; set; }
        public short BusinessSubtypeId { get; set; }
        public int CityId { get; set; }
        public decimal NetProfit { get; set; }
        public decimal RunningCost { get; set; }
        public sbyte Active { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual BusinessLocation BusinessLocation { get; set; }
        public virtual ICollection<LoanFromBusiness> LoanFromBusinesses { get; set; }
    }
}
