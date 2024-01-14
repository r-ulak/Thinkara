using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CasinoDTORepository : ICasinoDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        public CasinoDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public CasinoDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public string GetSlotMachineThreeList()
        {
            string slotList = cache.GetStringKey(AppSettings.RedisKeySlotMachineList);
            if (slotList == null)
            {
                SlotMachineThreeDTO machinelist = new SlotMachineThreeDTO();
                machinelist.SlotMachineList = spContext.GetSqlDataNoParms<SlotMachineThree>(
                AppSettings.SPGetSlotMachineThreeList
                ).ToArray();
                slotList = JsonConvert.SerializeObject(machinelist);
                cache.SetStringKey(AppSettings.RedisKeySlotMachineList, slotList);
            }
            return slotList;
        }

    }
}
