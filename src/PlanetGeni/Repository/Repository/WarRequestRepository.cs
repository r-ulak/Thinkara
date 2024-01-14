using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WarRequestRepository : IWarRequestRepository
    {
        private StoredProcedure spContext = new StoredProcedure();

        public WarRequestRepository()
        {
        }


        public RequestWarKey GetPendingWarRequest(string countryId, string targetCountryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmTargetCountryId", targetCountryId);

            RequestWarKey pendingWarRequest =
                spContext.GetSqlDataSignleRow<RequestWarKey>(AppSettings.SPGetPendingWarRequest, dictionary);
            return pendingWarRequest;

        }

        public bool SaveWarRequest(RequestWarKey warkeyRequest)
        {
            bool result = false;
            try
            {
                spContext.Add(warkeyRequest);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveWarRequest");
                return false;
            }
        }

        public RequestWarKey GetRequestWarKeyByTask(string taskId)
        {
            ///TODO check if they are eligible to pull in EDIT or read

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            RequestWarKey
            requestWarKey = spContext.GetByPrimaryKey<RequestWarKey>(dictionary);
            return requestWarKey;
        }
    }
}
