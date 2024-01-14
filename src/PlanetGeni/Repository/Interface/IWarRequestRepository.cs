using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IWarRequestRepository
    {
        RequestWarKey GetPendingWarRequest(string countryId, string targetCountryId);
        bool SaveWarRequest(RequestWarKey warkeyRequest);
        RequestWarKey GetRequestWarKeyByTask(string taskId);

    }
}
