using System;
using System.ComponentModel;

namespace DTO.Custom
{
    public class WebUserIndexDTO
    {
        public int UserId { get; set; }
        [DefaultValue("")]
        public string FullName { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Picture { get; set; }
        public string CountryId { get; set; }
        [DefaultValue("")]
        public string EmailId { get; set; }
    }
}
