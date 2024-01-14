using Common;
using DAO.Models;
using DTO.Custom;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Jobs;
using PlanetGeni.HttpHelper;

namespace StockBroker
{
    class StockBroker
    {

        static void Main(string[] args)
        {
            ProcessAll();
        }
        static void ProcessAll()
        {
            StockBrokerManager manager = new StockBrokerManager();
            DateTime dateTime = DateTime.UtcNow.AddDays(RulesSettings.StockDaysHistory);
            if (dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                dateTime.AddDays(-1);
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                dateTime.AddDays(-2);
            }
            Console.WriteLine(manager.ProcessAll(dateTime.ToString("yyyy-MM-dd")).ToString());

            Console.WriteLine("Response Saved");
        }
    }
}