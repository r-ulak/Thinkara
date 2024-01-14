using System;
namespace DTO.Custom
{
    public class RouletteDTO
    {
        public decimal BetAmount { get; set; }
        public short SelectedNumber { get; set; }
        public int WinNumber { get; set; }
        public decimal TotalAward { get; set; }
        public string CountryId { get; set; }
        public int UserId { get; set; }
    }
}
