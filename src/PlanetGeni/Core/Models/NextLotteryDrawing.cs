using System;

namespace DAO.Models
{
    public partial class NextLotteryDrawing
    {
        public string LotteryType { get; set; }
        public decimal LotteryPrice { get; set; }
        public DateTime NextDrawingDate { get; set; }
        public int DrawingId { get; set; }
    }
}
