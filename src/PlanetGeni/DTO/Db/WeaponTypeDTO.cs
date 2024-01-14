namespace DAO.Models
{
    public partial class WeaponTypeDTO
    {
        public short WeaponTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string ImageFont { get; set; }
        public string WeaponTypeCode { get; set; }
    }
}
