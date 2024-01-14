using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IWeaponDetailsDTORepository
    {

        int UpdateWeaponCondition();
        string GetWeaponCodesJson();
        void ClearCache();
        bool SaveWeaponCart(CountryWeapon[] weaponCartList, Guid taskId);
        IQueryable<WeaponInventoryDTO> GetWeaponInventory(string countryId, int lastWeaponId);
        string GetWeaponSummaryJson(string countryId);
        int GetWeaponAssetCount(string countryId);
        string GetTop10WeaponStackCountryJson();
        int GetCountryOffenseAssetRank(string countryCode);
        int GetCountryDefenseAssetRank(string countryCode);
        string GetSecurityProfile(string countryId);
    }
}
