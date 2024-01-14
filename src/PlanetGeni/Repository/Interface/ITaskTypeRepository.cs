using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITaskTypeRepository
    {
        string GetTaskDescriptionByType(int tasktypeId);

    }
}
