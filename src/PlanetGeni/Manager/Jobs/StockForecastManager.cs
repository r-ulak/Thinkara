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
    public class StockForecastManager
    {
        private IStockDTORepository _stockRepo;
        private string forecastDate;
        public StockForecastManager(DateTime date)
        {
            _stockRepo = new StockDTORepository();
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date.AddDays(-1);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date.AddDays(-2);
            }

            this.forecastDate = date.ToString("yyyy-MM-dd");

        }
        public void ProcessAll(int runId)
        {
            _stockRepo.SetStockForecast(forecastDate);
            _stockRepo.SendStcokForecastNotification();
        }


    }
}
