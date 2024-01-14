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
    public class BudgetStimulatorManager
    {
        ICountryBudgetDetailsDTORepository budgetRepo = new CountryBudgetDetailsDTORepository();
        public BudgetStimulatorManager()
        {

        }
        public void BudgetStimulus(int runId)
        {
            Console.WriteLine("Applying BudgetPopulationStimulator...");
            int count = budgetRepo.ApplyBudgetPopulationStimulator();
            Console.WriteLine("Applied  BudgetPopulationStimulator For {0}", count);
            Console.WriteLine("Applying BudgetWarStimulator...");
            budgetRepo.ApplyBudgetWarStimulator();
            Console.WriteLine("Applying BudgetStimulator...");
            count = budgetRepo.ApplyBudgetStimulator();
            Console.WriteLine("Applied BudgetStimulator For {0}", count);
        }
    }
}
