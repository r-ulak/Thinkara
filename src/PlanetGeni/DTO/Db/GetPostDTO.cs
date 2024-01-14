using System;
namespace DTO.Db
{
    public class GetPostDTO
    {
        public string LastPostId { get; set; }
        public DateTime? LastCreatedAt { get; set; }
        public int UserId { get; set; }
        public string CountryId { get; set; }
        public string PartyId { get; set; }
        public bool NewPost { get; set; }

    }
}
