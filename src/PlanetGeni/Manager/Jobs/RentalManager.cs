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
    public class RentalManager
    {
        private IMerchandiseDetailsDTORepository merRepo = new MerchandiseDetailsDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private decimal taxRate = 0;
        private int totalRenter = 0;
        private int totalCollector = 0;
        public RentalManager()
        {

        }
        public void PayCollectRental(int runId)
        {
            List<CountryCode> countries = JsonConvert.DeserializeObject<List<CountryCode>>(countryRepo.GetCountryCodes());
            ICountryTaxDetailsDTORepository rentalTax = new CountryTaxDetailsDTORepository();
            foreach (var item in countries)
            {
                taxRate = rentalTax.GetCountryTaxByCode(item.CountryId, AppSettings.TaxIncomeCode);

                Console.WriteLine("{0}      Processing  {1} {2}", item.CountryId, item.Code, item.CountryUserId);
                CollectRent(item);
                PayRent(item);
            }
            Console.WriteLine("Total Number of User Collecting Rents {0}", totalCollector);
            Console.WriteLine("Total Number of User Paying Rents {0}", totalRenter);


        }

        private void PayRent(CountryCode item)
        {
            int renters = merRepo.ProcessUserWithoutHouse(item.CountryId, item.CountryUserId);
            Console.WriteLine("Number of User Paying Rents {0}", renters);

            totalRenter += renters;
        }

        private void CollectRent(CountryCode item)
        {
            int collectors = merRepo.ProcessUserWithRentalProperty(item.CountryId, taxRate);
            Console.WriteLine("Number of User Collecting Rents {0}", collectors);
            totalCollector += collectors;
        }
    }
}
