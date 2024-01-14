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
    public class WebJobRepository
    {
        private StoredProcedure spContext = new StoredProcedure();

        public WebJobRepository()
        {
        }

        public WebJobHistory GetLastJobRun(sbyte jobId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmJobId", jobId);
            return spContext.GetSqlDataSignleRow<WebJobHistory>(AppSettings.SPGetLastWebJobRunTime, dictionary);
        }
        public int AddjobHistory(WebJobHistory history)
        {
            return spContext.Add(history);

        }

    }
}
