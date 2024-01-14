using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class NationalSecurityRules : IRules
    {
        private BuyWeaponDTO[] WepaonList;
        private List<WeaponType> WeaponCodeList;
        private int UserId;
        private string CountryId;
        public NationalSecurityRules()
        {

        }
        public NationalSecurityRules(BuyWeaponDTO[] wepaonList, string weaponCodes, string countryId, int userId)
        {
            WepaonList = wepaonList;
            WeaponCodeList = JsonConvert.DeserializeObject<List<WeaponType>>(weaponCodes);
            CountryId = countryId;
            UserId = userId;
        }

        public ValidationResult IsValid()
        {

            return ValidationResult.Success;

        }
        public CountryWeapon[] GetCountryWeapons()
        {
            CountryWeapon[] cartWeapons = new CountryWeapon[WepaonList.Length];

            for (int i = 0; i < WepaonList.Length; i++)
            {
                cartWeapons[i] = new CountryWeapon();
                cartWeapons[i].CountryId = CountryId;
                cartWeapons[i].UserId = UserId;
                cartWeapons[i].Quantity = WepaonList[i].Quantity;
                cartWeapons[i].WeaponTypeId = WepaonList[i].WeaponTypeId;
                cartWeapons[i].WeaponCondition = 100;
                cartWeapons[i].PurchasedPrice =
                    WeaponCodeList.First(x => x.WeaponTypeId == WepaonList[i].WeaponTypeId).Cost * WepaonList[i].Quantity;
                cartWeapons[i].PurchasedAt = DateTime.UtcNow;


            }

            return cartWeapons;

        }
        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }
    }
}
