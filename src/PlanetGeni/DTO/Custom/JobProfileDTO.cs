using System;
namespace DTO.Db
{
    public class JobProfileDTO
    {
        public string Title { get; set; }
        public string IndustryName { get; set; }
        public string JobTypeName { get; set; }
        public decimal Salary { get; set; }
        public string ImageFont { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal OverTimeRate { get; set; }
        public System.DateTime StartDate { get; set; }
        public decimal IncomeYearToDate { get; set; }
    }


}
