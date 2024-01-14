using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PoliticalParty
    {
        public short PartyId { get; set; }
        public string PartyName { get; set; }
        public int PartyFounder { get; set; }
        public string LogoPictureId { get; set; }
        public bool Active { get; set; }
    }
}
