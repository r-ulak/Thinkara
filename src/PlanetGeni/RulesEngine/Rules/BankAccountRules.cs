using Common;
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
    public class BankAccountRules : IRules
    {

        public BankAccountRules()
        {

        }
        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }
        public ValidationResult IsValidBuy(ref BuySellMetalDTO metal,
            UserBankAccount bankAc, List<CapitalType> capitalTypes)
        {
            decimal total = 0;
            total += metal.GoldDelta * capitalTypes.Find(f => f.Name == "Gold").Cost;
            total += metal.SilverDelta * capitalTypes.Find(f => f.Name == "Silver").Cost;
            if (metal.SilverDelta <= 0 && metal.GoldDelta <= 0)
            {
                return new ValidationResult(("your cart is empty"));
            }

            if (total > bankAc.Cash)
            {
                return new ValidationResult(("you do not have enough cash to buy"));
            }
            metal.Delta = -total;

            return ValidationResult.Success;
        }
        public ValidationResult IsValidSell(ref BuySellMetalDTO metal,
    UserBankAccount bankAc, List<CapitalType> capitalTypes)
        {

            if (metal.SilverDelta >= 0 && metal.GoldDelta >= 0)
            {
                return new ValidationResult(("your cart is empty"));
            }
            if (Math.Abs(metal.GoldDelta) > bankAc.Gold)
            {
                return new ValidationResult(("you do not have enough Gold to sell"));
            }
            if (Math.Abs(metal.SilverDelta) > bankAc.Silver)
            {
                return new ValidationResult(("you do not have enough Silver to sell"));
            }
            decimal total = 0;
            total += metal.GoldDelta * capitalTypes.Find(f => f.Name == "Gold").Cost;
            total += metal.SilverDelta * capitalTypes.Find(f => f.Name == "Silver").Cost;

            metal.Delta = Math.Abs(total);

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
