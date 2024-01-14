using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DAO.DAO.ComplexType;
using DAO.Models;
using PlanetX.ContentProviders.Core;
using PlanetX.Infrastructure;
using PlanetX.BRO;
using System.Text.RegularExpressions;
using System.Threading;
using Test.TrendingServiceReference;
using DAO.DAO;
using PlanetX2012.Models.DAO;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            {
                StoredProcedure sp = new StoredProcedure();

          
                Console.WriteLine("Loading Data...");
                List<CityCountryID> CityList = sp.GetSqlDataNoParms<CityCountryID>("GetCityList").ToList();
                //var unique = new HashSet<CityCountryID>(CityList);
                Console.WriteLine("Loading Completed/ Checking Duplicates");
                List<CityCountryID> unique = CityList.Distinct().ToList();
                //List<CityCountryID> duplicates = CityList.GroupBy(x => new { x.City, x.CountryId })
                //             .Where(g => g.Count() > 1)
                //             .Select(g => new CityCountryID() { City = g.Key.City, CountryId = g.Key.CountryId })
                //             .ToList();

                Console.WriteLine("Checking Duplicates Completed/ PrintingResult");
                foreach (CityCountryID item in unique)
                {
                    Console.WriteLine("{0},{1}",  item.CountryId, item.City.TrimEnd('\r', '\n'));

                }
            }
        }


        private static void DowWork(object state)
        {
            Console.WriteLine(DateTime.Now);


        }
    }
}