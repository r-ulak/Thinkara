using System;
namespace DAO.Models
{
    public partial class UserBankAccount
    {
        public int UserId { get; set; }
        public decimal Cash { get; set; }
        public decimal Gold { get; set; }
        public decimal Silver { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
