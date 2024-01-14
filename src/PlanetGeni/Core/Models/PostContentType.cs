using System;

namespace DAO.Models
{
    public partial class PostContentType
    {
        public sbyte PostContentTypeId { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public string FontCss { get; set; }
    }
}
