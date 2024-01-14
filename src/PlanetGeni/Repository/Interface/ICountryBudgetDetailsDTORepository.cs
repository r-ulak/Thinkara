using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICountryBudgetDetailsDTORepository
    {
        void ClearCache();
        string GetCountBudgetPercenTile(string countryId);
        string GetCountryBudget(string countryid);
        CountryBudgetByType GetCountryBudgetByType(string countryid, int budgetType);
        bool SaveBudget(CountryBudgetDetailsDTO budgetDetails, string countryId, int requestoruserId,
      string fullName, Guid taskId);
        CountryBudget GetCountryBudgetByTaskId(Guid taskId);
        CountryBudgetDetailsDTO GetCountryBudgetByTask(string taskId);
        int GetCountryBudgetRank(string countryCode);
        decimal GetCountryBudgetValue(string countryCode);
        int ApproveNewBudgetAmendments(string currentTaskId, string newTaskId);
        int GetCountrySafestRank(string countryCode);
        int ApplyBudgetStimulator();
        void ApplyBudgetWarStimulator();
        int ApplyBudgetPopulationStimulator();
        void ReCalculateBudget();
    }
}
