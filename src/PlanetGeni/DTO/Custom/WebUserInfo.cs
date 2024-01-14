using System;

namespace DTO.Custom
{
    public class WebUserInfo
    {
        public string FullName { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Picture { get; set; }
        public sbyte OnlineStatus { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public int UserLevel { get; set; }
        public decimal NetWorth { get; set; }
        public int UserId { get; set; }
        public string LoginProvider { get; set; }
        public string IsLeader { get; set; }
    }
}
