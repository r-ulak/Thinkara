using System;
namespace DTO.Custom
{
    public class AppliedJobDTO
    {
        public Guid TaskId { get; set; }
        public short JobCodeId { get; set; }
        public int UserId { get; set; }
    }
}
