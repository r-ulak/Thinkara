using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PickFive
    {
        public Guid PickFiveId { get; set; }
        public int DrawingId { get; set; }
        public int UserId { get; set; }
        public sbyte Number1 { get; set; }
        public sbyte Number2 { get; set; }
        public sbyte Number3 { get; set; }
        public sbyte Number4 { get; set; }
        public sbyte Number5 { get; set; }
    }
}
