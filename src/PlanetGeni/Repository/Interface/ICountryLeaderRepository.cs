using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICountryLeaderRepository
    {
        void ClearCache(string countryId);
        string GetActiveSeneatorJson(string countryId);
        int GetTotalActiveSeneator(string countryId);
        string GetActiveLeadersProfile(string countryId);
        IEnumerable<CountryLeader> GetActiveSeneator(string countryId);

    }
}
