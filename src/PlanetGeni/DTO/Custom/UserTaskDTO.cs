using System;
namespace DTO.Custom
{
    public class UserTaskDTO
    {
        public Guid TaskId { get; set; }
        public int UserId { get; set; }
        public short DefaultResponse { get; set; }
        public short TaskTypeId { get; set; }
    }
}
