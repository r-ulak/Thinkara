using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ElectionDonation
    {
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public int RequestedTo { get; set; }
        public decimal AmountRecieved { get; set; }
        public bool DonationDenied { get; set; }
    }
}
