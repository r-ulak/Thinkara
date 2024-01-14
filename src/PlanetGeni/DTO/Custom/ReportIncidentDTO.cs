using Common;
using System;
namespace DTO.Custom
{
    public class ReportIncidentDTO
    {
        public int VictimId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncidentDate { get; set; }
        public short MerchandiseTypeId { get; set; }
        public string Name { get; set; }
        public sbyte Quantity { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public string IncidentType { get; set; }
        public sbyte MerchandiseTypeCode { get; set; }
        public ReportIncidentDTO()
        {
            ImageFont = "icon-money28";
            Name = "Cash";
            Description = "Cash is green but Gold is golden.";
            Quantity= RulesSettings.RobberyMaxQuantity;
        }
    }

}
