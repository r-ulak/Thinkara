using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PickFiveWinNumber
    {
        public int DrawingId { get; set; }
        public sbyte Number1 { get; set; }
        public sbyte Number2 { get; set; }
        public sbyte Number3 { get; set; }
        public sbyte Number4 { get; set; }
        public sbyte Number5 { get; set; }
        public System.DateTime DrawingDate { get; set; }
    }
}
