using DAO.Models;
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
    public class MerchandiseRules : IRules
    {
        public BuySellMerchandiseDTO[] MerchandiseList;
        private decimal TaxRate;
        public List<MerchandiseCache> MerchandiseCodeList;
        private bool HasAllMerchandiseList;
        private UserBankAccount BuyerBankAccount;

        public MerchandiseRules()
        {

        }
        public MerchandiseRules(BuySellMerchandiseDTO[] merchandiseList,
            UserBankAccount buyerBankAccount,
          decimal taxRate)
        {
            MerchandiseList = merchandiseList;
            TaxRate = taxRate;
            BuyerBankAccount = buyerBankAccount;
        }

        public MerchandiseRules(bool hasAllMerchandiseList)
        {
            HasAllMerchandiseList = hasAllMerchandiseList;
        }
        public ValidationResult IsValid()
        {
            decimal totalwithtax = 0;
            foreach (var item in MerchandiseList)
            {
                MerchandiseCache merchandise =
                     MerchandiseCodeList.First(x => x.MerchandiseTypeId == item.MerchandiseTypeId);
                if (merchandise == null)
                {
                    return new ValidationResult(("invalid item detected in buy cart"));
                }
                totalwithtax += merchandise.Cost * item.Quantity;
                item.Cost = merchandise.Cost * item.Quantity;
                item.Tax = item.Cost * TaxRate / 100;
            }
            totalwithtax = totalwithtax * (1 + TaxRate / 100);
            if (totalwithtax > BuyerBankAccount.Cash)
            {
                return new ValidationResult(("not enough cash to buy"));
            }
            return ValidationResult.Success;
        }

        public ValidationResult IsValidSellCart()
        {
            if (HasAllMerchandiseList == false)
            {
                return new ValidationResult(("Invalid Item detected in sell cart"));
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
