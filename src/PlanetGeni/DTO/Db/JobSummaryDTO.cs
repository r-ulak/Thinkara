using System;
namespace DTO.Db
{
    public class JobSummaryDTO
    {
        public int TotalJobsAccepted { get; set; }
        public int TotalJobsRejcted { get; set; }
        public int TotalJobsDenied { get; set; }
        public int TotalJobsPending { get; set; }
        public int TotalJobsOpenOffer { get; set; }
        public decimal TotalIncomeYTD { get; set; }
        public decimal TotalIncomeTaxYTD { get; set; }
    }
}
