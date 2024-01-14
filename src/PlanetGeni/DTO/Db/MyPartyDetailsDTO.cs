using System;
namespace DTO.Db
{
    public class MyPartyDetailsDTO
    {
        public short PartyId { get; set; }
        public PartyMemberDTO[] Founders { get; set; }        
        public ElectionWinLossDTO[] PartyBoxScore { get; set; }
    }
}
