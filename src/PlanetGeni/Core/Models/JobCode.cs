using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class JobCode
    {
        public short JobCodeId { get; set; }
        public string Title { get; set; }
        public string JobType { get; set; }
        public short Duration { get; set; }
        public sbyte CheckInDuration { get; set; }
        public int MinimumMatchScore { get; set; }
        public sbyte MaxHPW { get; set; }
        public decimal OverTimeRate { get; set; }
        public sbyte IndustryId { get; set; }
        public short ReqMajorId { get; set; }
        public sbyte ReqDegreeId { get; set; }
        public short IndustryExperience { get; set; }
        public short JobExperience { get; set; }
    }
}
