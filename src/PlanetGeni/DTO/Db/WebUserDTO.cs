namespace DAO.Models
{
    public partial class WebUserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public sbyte OnlineStatus { get; set; }
        public string CountryId { get; set; }
    }
}
