
using System;
namespace DTO.Db
{
    public class VoteResponseDTO
    {
        public int[] ChoiceIds { get; set; }
        public int ChoiceRadioId { get; set; }
        public Guid TaskId { get; set; }
        public int TaskTypeId { get; set; }
        public bool IsIncomepleteTask { get; set; }

    }
}
