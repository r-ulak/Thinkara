using System;

namespace DTO.Db
{
    public class PartySearchDTO
    {
        public string[] AgendaType { get; set; }
        public int PartySizeRangeUp { get; set; }
        public int PartySizeRangeDown { get; set; }
        public int PartyVictoryRangeUp { get; set; }
        public int PartyVictoryRangeDown { get; set; }
        public decimal PartyFeeRangeUp { get; set; }
        public decimal PartyFeeRangeDown { get; set; }  
        public decimal PartyWorthRangeUp { get; set; }
        public decimal PartyWorthRangeDown { get; set; }
        public DateTime LastStartDate { get; set; }
    }
}
