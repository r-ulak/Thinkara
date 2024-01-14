
using System;
namespace DAO.Models
{
    public partial class ContactSource
    {
        public sbyte ProviderId { get; set; }
        public int UserId { get; set; }
        public int Total { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
