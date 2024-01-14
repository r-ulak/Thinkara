using System;
namespace DTO.Custom
{
    public class EnrollDegreeDTO
    {
        public int MajorId { get; set; }
        public int DegreeId { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int Duration { get; set; }
    }
}
