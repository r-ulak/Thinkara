using System;
namespace DTO.Custom
{
    public class CrimeIncidentDTO
    {
        public Guid IncidentId { get; set; }
        public int UserId { get; set; }
        public int SuspectReportingUserId { get; set; }
        public int VictimId { get; set; }
        public int SuspectId { get; set; }
        public bool IsFriend { get; set; }
        public decimal Amount { get; set; }
        public decimal MaxAllowedAmount { get; set; }
        public short MerchandiseTypeId { get; set; }
        public bool RobbedRecently { get; set; }
        public bool SuspectAleadyNotified { get; set; }
        public string StolenAsset{ get; set; }
    }
}
