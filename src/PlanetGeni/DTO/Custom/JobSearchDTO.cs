using System;
namespace DTO.Custom
{
    public class JobSearchDTO
    {
        public int[] Industry { get; set; }
        public int[] Major { get; set; }
        public string[] JobType { get; set; }
        public decimal SalaryLowerRange { get; set; }
        public decimal SalaryHigherRange { get; set; }
        public int LastJobCodeId { get; set; }
        public int OverTime { get; set; }
    }
}
