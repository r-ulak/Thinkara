using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class EducationRules : IRules
    {
        public EnrollDegreeDTO[] EnrollDegreeList;
        private decimal TaxRate;
        private List<MajorCode> MajorCodeList;
        private bool MeetsPreRequisite;
        private UserBankAccount BuyerBankAccount;

        public EducationRules()
        {

        }
        public EducationRules(EnrollDegreeDTO[] enrollDegreeList, List<MajorCode> majorCodeList,
            UserBankAccount buyerBankAccount, bool meetsPreRequisite,
          decimal taxRate)
        {
            EnrollDegreeList = enrollDegreeList;
            TaxRate = taxRate;
            MajorCodeList = majorCodeList;
            BuyerBankAccount = buyerBankAccount;
            MeetsPreRequisite = meetsPreRequisite;
        }


        public ValidationResult IsValid()
        {
            decimal totalwithtax = 0;
            DateTime dateTime = DateTime.UtcNow;
            if (MeetsPreRequisite == false)
            {
                    return new ValidationResult(("found major that does not meet prerequisite"));
                
            }
            foreach (EnrollDegreeDTO item in EnrollDegreeList)
            {
                decimal cost = MajorCodeList.Find(x => x.MajorId == item.MajorId).Cost;
                int duration = MajorCodeList.Find(x => x.MajorId == item.MajorId).Duration;

                if (cost == 0)
                {
                    return new ValidationResult(("invalid major detected trying to enroll"));
                }
                cost = cost * (1 + item.DegreeId) ;
                totalwithtax += cost * (1 + TaxRate / 100);
                item.Cost = cost;
                item.Duration = duration;
                item.Status = "I";
                item.CreatedAt = dateTime;

            }

            if (totalwithtax > BuyerBankAccount.Cash)
            {
                return new ValidationResult(("not enough cash to buy"));
            }
            return ValidationResult.Success;
        }


        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }



    }
}
