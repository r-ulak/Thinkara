using System;
namespace DTO.Db
{
    public class ElectionWinLossDTO
    {
        public short PartyId { get; set; }
        public string PostionName { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
    }
}

