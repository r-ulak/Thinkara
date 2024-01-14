using DAO.Models;
using System;
namespace DTO.Db
{
    public class EducationDTO : MajorCode
    {
        public sbyte DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string DegreeImageFont { get; set; }
        public decimal CompletionCost { get; set; }
        public System.DateTime ExpectedCompletion { get; set; }
        public System.DateTime NextBoostAt { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string KnobColor { get; set; }
        public decimal CompletionPercent { get; set; }
        public sbyte DegreeTextFont { get; set; }
        public sbyte CertificateTextFont { get; set; }

    }


}
