using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PickFiveWinner
    {
        public int WinId { get; set; }
        public int DrawingId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
