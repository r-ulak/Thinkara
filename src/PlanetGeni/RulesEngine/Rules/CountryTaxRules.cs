using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class CountryTaxRules : IRules
    {
        public string CountryId { get; set; }
        public CountryTaxDetailsDTO TaxDetails { get; set; }

        public CountryTaxRules()
        {

        }
        public CountryTaxRules(string countryId, CountryTaxDetailsDTO taxdetails)
        {
            CountryId = countryId;
            TaxDetails = taxdetails;
        }

        public ValidationResult IsValid()
        {
            if (TaxDetails.AllowEdit == false)
            {
                return new ValidationResult("Access Denied"); ;
            }
            if (CountryId != TaxDetails.CountryId)
            {
                return new ValidationResult("Invalid User"); ;
            }
            return ValidationResult.Success;
        }


    }
}
