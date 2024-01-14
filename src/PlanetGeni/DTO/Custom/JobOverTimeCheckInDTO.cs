using System;
namespace DTO.Custom
{
    public class JobOverTimeCheckInDTO
    {
        public System.DateTime NextOverTimeCheckIn { get; set; }
        public Guid TaskId { get; set; }
    }
}
