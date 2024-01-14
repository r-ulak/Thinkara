
using DAO.Models;
namespace DTO.Db
{
    public class Pick5WinDTO 
    {
        public int DrawingId { get; set; }
        public decimal Amount { get; set; }
        public sbyte Number1 { get; set; }
        public sbyte Number2 { get; set; }
        public sbyte Number3 { get; set; }
        public sbyte Number4 { get; set; }
        public sbyte Number5 { get; set; }
    }
}
