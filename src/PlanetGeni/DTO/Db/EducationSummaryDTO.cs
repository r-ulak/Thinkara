using DTO.Custom;
using System;
namespace DAO.Models
{
    public partial class EducationSummaryDTO:DegreeCode
    {
        public string Status { get; set; }
        public Int64 Total { get; set; }
    }
}
