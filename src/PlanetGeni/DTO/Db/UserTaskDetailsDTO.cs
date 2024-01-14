using System;
namespace DTO.Db
{
    public partial class UserTaskDetailsDTO
    {
        public Guid TaskId { get; set; }
        public sbyte CompletionPercent { get; set; }
        public bool Flagged { get; set; }
        public string Status { get; set; }
        public string Parms { get; set; }
        public DateTime DueDate { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
