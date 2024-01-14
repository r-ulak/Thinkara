
using System;
namespace DAO.DAO
{


    public partial class FinanceContent
    {
        public Nullable<decimal> creditTotal { get; set; }
        public Nullable<decimal> debitTotal { get; set; }
        public Nullable<decimal> businessLoanTotal { get; set; }
        public Nullable<decimal> personLoanTotal { get; set; }
    }
}
