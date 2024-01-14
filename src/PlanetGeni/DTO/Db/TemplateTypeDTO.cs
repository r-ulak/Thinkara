
using DAO.Models;
using System.Collections.Generic;
namespace DTO.Db
{
    public class CodeTableDTO
    {
        public IEnumerable<PostContentType> PostContentType { get; set; }
        public IEnumerable<NotificationType> NotificationType { get; set; }
        public IEnumerable<TaskType> TaskType { get; set; }
        public IEnumerable<ContactProvider> ContactProvider { get; set; }

    }
}
