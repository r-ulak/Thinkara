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
    public class TaskTypeRepository : ITaskTypeRepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public TaskTypeRepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public TaskTypeRepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetTaskDescriptionByType(int tasktypeId)
        {
            string tasktypeData = cache.GetStringKey(AppSettings.RedisKeyTaskType + tasktypeId.ToString());
            if (tasktypeData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskTypeId", tasktypeId);

                tasktypeData = JsonConvert.SerializeObject(spContext.GetByPrimaryKey<TaskType>(dictionary));
                cache.SetStringKey(AppSettings.RedisKeyTaskType + tasktypeId.ToString(), tasktypeData);
            }
            return (tasktypeData);
        }

        public void ClearCache()
        {
            cache.Invalidate(AppSettings.RedisKeyTaskType);
        }
    }
}
