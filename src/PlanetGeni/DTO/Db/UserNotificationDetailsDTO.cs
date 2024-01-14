using System;
using System.ComponentModel;
namespace DTO.Db
{
    public partial class UserNotificationDetailsDTO
    {
        public Guid NotificationId { get; set; }
        public string Parms { get; set; }
        public string ImageFont { get; set; }
        public bool HasTask { get; set; }
        public short Priority { get; set; }
        [DisplayName("OnClickUrl")]
        public string OnClickUrl { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
