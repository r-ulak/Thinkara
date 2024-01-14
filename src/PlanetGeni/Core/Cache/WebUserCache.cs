namespace DAO.Models
{
    public partial class WebUserCache
    {
        public string FullName { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Picture { get; set; }
        public sbyte OnlineStatus { get; set; }
        public int UserLevel { get; set; }
        public string CountryId { get; set; }
        public string IsLeader { get; set; }
    }
}
