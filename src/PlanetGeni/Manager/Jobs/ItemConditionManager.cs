using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class ItemConditionManager
    {
        IMerchandiseDetailsDTORepository merchandiseRepo = new MerchandiseDetailsDTORepository();
        IWeaponDetailsDTORepository weaponRepo = new WeaponDetailsDTORepository();
        public ItemConditionManager()
        {

        }
        public void UpdateItemCondition(int runId)
        {
            Console.WriteLine("Updating  Merchandise Condition...");
            int count = merchandiseRepo.UpdateMerchandiseCondition();
            Console.WriteLine("Updated {0} Merchandise User rows",count);

            Console.WriteLine("Updating  Weapon Condition...");
            count = weaponRepo.UpdateWeaponCondition();
            Console.WriteLine("Updated {0} Weapon Condtion", count);

        
        }


    }
}
