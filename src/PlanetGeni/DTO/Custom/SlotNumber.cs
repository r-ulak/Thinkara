using System;
namespace DTO.Custom
{
    public class SlotNumber
    {
        public int UserId { get; set; }
        public string CountryId { get; set; }
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Number3 { get; set; }
        public decimal BetAmount { get; set; }
        public sbyte Match { get; set; }
        public decimal TotalAward { get; set; }
    }
}
