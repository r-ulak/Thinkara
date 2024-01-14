using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Advertisement
    {
        public Guid AdvertisementId { get; set; }
        public int UserId { get; set; }
        public bool AdsTypeEmail { get; set; }
        public bool AdsTypeFeed { get; set; }
        public bool AdsTypePartyMember { get; set; }
        public bool AdsTypeCountryMember { get; set; }
        public sbyte AdsFrequencyTypeId { get; set; }
        public bool DaysS { get; set; }
        public bool DaysM { get; set; }
        public bool DaysT { get; set; }
        public bool DaysW { get; set; }
        public bool DaysTh { get; set; }
        public bool DaysF { get; set; }
        public bool DaysSa { get; set; }
        public int AdTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PreviewMsg { get; set; }
        public string Message { get; set; }
        public decimal Cost { get; set; }
    }
}
