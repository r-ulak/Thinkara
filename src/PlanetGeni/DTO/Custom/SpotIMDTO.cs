using System;
namespace DTO.Custom
{
    public class SpotIMDTO
    {
        public string display_name { get; set; }
        public string company_user_id { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public SpotIMDTO(string fullName, int userId, string picture)
        {
            display_name = fullName;
            company_user_id = "planet_" + userId.ToString();
            email = userId.ToString() + "@thinkara.com";
            image = picture;
        }
    }
}
