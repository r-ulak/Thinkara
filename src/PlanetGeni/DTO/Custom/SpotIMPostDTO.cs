using System;
namespace DTO.Custom
{
    public class SpotIMPostDTO
    {
        public string sso_type { get; set; }
        public SpotIMDTO user_info { get; set; }
        public string spot_to_join { get; set; }
        public SpotIMPostDTO()
        {
            sso_type = "general";
        }
    }
}
