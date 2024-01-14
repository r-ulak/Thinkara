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
    public class LotteryRules : IRules
    {
        public bool HasCurrentOrAppliedJob;
        private PickThree LotteryPickThree;
        private PickFive LotteryPickFive;
        public LotteryRules()
        {

        }
        public LotteryRules(
            PickThree pickThree)
        {
            LotteryPickThree = pickThree;

        }
        public LotteryRules(
    PickFive pickFive)
        {
            LotteryPickFive = pickFive;

        }

        public ValidationResult IsValid()
        {

            return ValidationResult.Success;
        }

        public ValidationResult IsValidPickThree()
        {
            if (!(LotteryPickThree.Number1 >= 1 && LotteryPickThree.Number1 <= 10 &&
                LotteryPickThree.Number2 >= 1 && LotteryPickThree.Number2 <= 10 &&
                LotteryPickThree.Number3 >= 1 && LotteryPickThree.Number3 <= 10
                ))
            {
                return new ValidationResult(("invalid numbers, must be in range of 1 to 10"));
            }
            return ValidationResult.Success;
        }
        public ValidationResult IsValidPickFive()
        {
            if (!(LotteryPickFive.Number1 >= 1 && LotteryPickFive.Number1 <= 10 &&
                LotteryPickFive.Number2 >= 1 && LotteryPickFive.Number2 <= 10 &&
                LotteryPickFive.Number3 >= 1 && LotteryPickFive.Number3 <= 10 &&
                LotteryPickFive.Number4 >= 1 && LotteryPickFive.Number4 <= 10 &&
                LotteryPickFive.Number5 >= 1 && LotteryPickFive.Number5 <= 10
                ))
            {
                return new ValidationResult(("invalid numbers, must be in range of 1 to 10"));
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
