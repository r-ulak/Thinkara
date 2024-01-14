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
    public class StockDividendManager
    {
        IStockDTORepository stockRepo = new StockDTORepository();
        public StockDividendManager()
        {

        }
        public void PayStockDividend(int runId)
        {
            Console.WriteLine("Paying Stock Dividend...");
            stockRepo.PayStockDividend();
            Console.WriteLine("Finished Paying Stock Dividend");    
        }
    }
}
