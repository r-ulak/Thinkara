using System;
namespace DTO.Db
{
    public class JobCodeDTO
    {
        public short JobCodeId { get; set; }
        public string Title { get; set; }
        public string JobType { get; set; }
        public short Duration { get; set; }
        public sbyte CheckInDuration { get; set; }
        public int MinimumMatchScore { get; set; }
        public sbyte MaxHPW { get; set; }
        public decimal OverTimeRate { get; set; }
        public string IndustryName { get; set; }
        public string ReqMajorName { get; set; }
        public string ReqDegreeName { get; set; }
        public short IndustryExperience { get; set; }
        public decimal Salary { get; set; }
        public short JobExperience { get; set; }
        public string ImageFont { get; set; }
    }


}
