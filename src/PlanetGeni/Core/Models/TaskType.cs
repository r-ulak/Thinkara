using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class TaskType
    {
        public int TaskTypeId { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public sbyte ChoiceType { get; set; }
        public sbyte MaxChoiceCount { get; set; }
    }
}
