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

namespace Manager.BudgetSpender
{
    public class BudgetSpenderManager
    {
        IEducationDTORepository educationRepo = new EducationDTORepository();
        ICountryBudgetDetailsDTORepository countryBudgetRepo = new CountryBudgetDetailsDTORepository();
        IStockDTORepository stockRepo = new StockDTORepository();
        IJobDTORepository jobRepo = new JobDTORepository();
        IPostCommentDTORepository postRepo = new PostCommentDTORepository();
        public BudgetSpenderManager()
        {

        }
        public void RunBudgetSpendor(int runId)
        {
            Console.WriteLine("Applying education Credits fro last month Enrolled users...");
            educationRepo.GiveEducationCreditForCountry();
            countryBudgetRepo.ClearCache();
            
            Console.WriteLine("Recaculating and distributing Cash and Amount Left from Last Budget... ");
            countryBudgetRepo.ReCalculateBudget();
            
            Console.WriteLine("Cancel StockOrder For Budget ... ");
            stockRepo.CancelStockOrderForBudget();
            
            Console.WriteLine("Buy Stocks... ");
            stockRepo.BuyStockOrderForBudget();
            
            Console.WriteLine("Increase Salary and Job Qty... ");
            jobRepo.IncreaseSalaryBudget();
            
            Console.WriteLine("Increase Army Job Qty... ");
            jobRepo.IncreaseArmyJob();


            Console.WriteLine("Increase Leaders Salary.. ");
            jobRepo.IncreaseLeadersSalary();

            Console.WriteLine("Clear Job Cahce... ");
            jobRepo.ClearCache();
            
            Console.WriteLine("Notify Post... ");
            postRepo.SendBudgetImpNotify();


        }
    }
}
		