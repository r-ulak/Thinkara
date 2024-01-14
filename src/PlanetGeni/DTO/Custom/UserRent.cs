using System;

namespace DTO.Custom
{
    public class UserRent
    {
        public string FullName { get; set; }
        public string Picture { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public int UserId { get; set; }
        public decimal TotalRental { get; set; }
    }
}
