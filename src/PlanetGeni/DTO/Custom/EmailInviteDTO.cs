using DAO.Models;
using System;
namespace DTO.Db
{
    public class EmailInviteDTO
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public ContactDTO[] InvitationList { get; set; }
        public Guid InvitationId { get; set; }
    }
}
