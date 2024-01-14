using System;
namespace DTO.Custom
{
    public class UserByNotification
    {
        public short NotificationTypeId { get; set; }
        public string Parms { get; set; }
        public int UserId { get; set; }
        public string NameFirst { get; set; }
        public string EmailId { get; set; }
    }
}
