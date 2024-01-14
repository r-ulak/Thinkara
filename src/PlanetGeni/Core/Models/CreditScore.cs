using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CreditScore
    {
        public int UserId { get; set; }
        public decimal Score { get; set; }
    }
}
