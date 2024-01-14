using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Advertisement
    {
        public int AdvertisementId { get; set; }
        public int UserId { get; set; }
        public string AdsTypeEmail { get; set; }
        public string AdsTypeFeed { get; set; }
        public string AdsTypeEmailAll { get; set; }
        public sbyte EventTypeId { get; set; }
        public Nullable<sbyte> AdsFrequencyTypeId { get; set; }
        public string DaysS { get; set; }
        public string DaysM { get; set; }
        public string DaysT { get; set; }
        public string DaysW { get; set; }
        public string DaysTh { get; set; }
        public string DaysF { get; set; }
        public string DaysSa { get; set; }
        public Nullable<System.TimeSpan> AdTime { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
    }
}
