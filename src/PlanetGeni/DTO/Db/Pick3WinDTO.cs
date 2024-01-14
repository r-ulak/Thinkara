
using DAO.Models;
namespace DTO.Db
{
    public class Pick3WinDTO 
    {
        public int DrawingId { get; set; }
        public decimal Amount { get; set; }
        public sbyte Number1 { get; set; }
        public sbyte Number2 { get; set; }
        public sbyte Number3 { get; set; }
    }
}
