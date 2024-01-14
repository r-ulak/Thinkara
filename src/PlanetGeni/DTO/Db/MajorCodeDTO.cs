using DAO.Models;
using System;
namespace DTO.Db
{
    public class MajorCodeDTO : MajorCode
    {
        public Int64 DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string DegreeImageFont { get; set; }
        public sbyte DegreeRank { get; set; }
        public string Status { get; set; }
    }


}
