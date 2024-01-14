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
    public interface ICountryTaxDetailsDTORepository
    {
        string GetRevenueByCountry(string countryId);
        CountryTaxDetailsDTO GetCountryTax(string countryid);
        bool SaveTax(CountryTaxDetailsDTO TaxDetails,
            string countryId,
            int requestoruserId,
            string fullName, Guid taskId);
        int ApproveNewTaxAmendments(string currentTaskId, string newTaskId);
        CountryTaxDetailsDTO GetCountryTaxByTask(string taskId);
        CountryTax GetCountryTaxByTaskId(string taskId);
        decimal GetCountryTaxByCode(string countryid, int taxcode);
    }
}
