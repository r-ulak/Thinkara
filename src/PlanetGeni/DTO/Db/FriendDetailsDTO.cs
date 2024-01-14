namespace DTO.Db
{
    public class FriendDetailsDTO
    {
        public int FriendUserId { get; set; }
        public string RelationDirection { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public sbyte OnlineStatus { get; set; }
        public string CountryId { get; set; }


    }
}
