using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Repository;
using Common;
namespace RulesEngine
{
    public class GiftRules : IRules
    {
        private GiftDTO SendingGift { get; set; }
        public UserBankAccount SenderBankAccount { get; set; }
        private decimal MerchandiseTotal;
        private bool HasAllMerchandiseList;
        private GiftRateDTO GiftRate { get; set; }
        private int NumberOfItemsAboveRange { get; set; }
        private ResourceManager
             resmgr = new ResourceManager("RulesEngine.ValidationMessage",
                               Assembly.GetExecutingAssembly());
        public GiftRules()
        {
        }
        public GiftRules(GiftDTO gift,
            UserBankAccount senderBankAccount, GiftRateDTO giftRate)
        {
            SendingGift = gift;
            SenderBankAccount = senderBankAccount;
            GiftRate = giftRate;

        }
        public GiftRules(GiftDTO gift,
    UserBankAccount senderBankAccount, decimal merchandiseTotal,
            GiftRateDTO giftRate,
            bool hasAllMerchandiseList,
            int numberOfItemsAboveRange)
        {
            SendingGift = gift;
            SenderBankAccount = senderBankAccount;
            GiftRate = giftRate;
            HasAllMerchandiseList = hasAllMerchandiseList;
            MerchandiseTotal = merchandiseTotal;
            NumberOfItemsAboveRange = numberOfItemsAboveRange;
        }
        public ValidationResult IsValid()
        {
            if (AllowUpdateInsert() == false)
                return new ValidationResult("AccessDenied");
            int totalRecipent = SendingGift.ToId.Count + SendingGift.NationId.Count;
            if (totalRecipent == 0)
            {
                return new ValidationResult("No Recipent Selected");
            }
            if (SendingGift.Gold * totalRecipent > SenderBankAccount.Gold)
            {
                return new ValidationResult("Not Enough Gold to Send");
            }
            decimal goldValue = SendingGift.Gold * GiftRate.CurrentGoldValue * totalRecipent;
            decimal goldTax = GiftRate.TaxRate * goldValue / 100;
            if (SenderBankAccount.Cash < goldTax)
            {
                return new ValidationResult("Not Enough Cash For Tax");
            }
            SenderBankAccount.Cash -= goldTax;
            SenderBankAccount.Gold -= SendingGift.Gold * totalRecipent;

            if (SendingGift.Silver * totalRecipent > SenderBankAccount.Silver)
            {
                return new ValidationResult("Not Enough Silver to Send");
            }
            decimal silverValue = SendingGift.Silver * GiftRate.CurrentSilverValue * totalRecipent;
            decimal silverTax = GiftRate.TaxRate * silverValue / 100;
            if (SenderBankAccount.Cash < silverTax)
            {
                return new ValidationResult("Not Enough Cash For Tax");
            }
            SenderBankAccount.Cash -= silverTax;
            SenderBankAccount.Silver -= SendingGift.Silver * totalRecipent;


            decimal totalcashvalue = (1 + GiftRate.TaxRate / 100) * SendingGift.Cash * totalRecipent;
            if (SenderBankAccount.Cash < totalcashvalue)
            {
                return new ValidationResult("Not Enough Cash For Tax");
            }
            SenderBankAccount.Cash -= totalcashvalue;


            return ValidationResult.Success;
        }
        public ValidationResult IsValidPropertyGift()
        {
            if (AllowUpdateInsert() == false)
                return new ValidationResult("AccessDenied");
            if (SendingGift.ToId.Count == 0)
            {
                return new ValidationResult("No Recipent Selected");

            }
            decimal cashTotal = SendingGift.ToId.Count * MerchandiseTotal * GiftRate.TaxRate / 100;
            if (cashTotal > SenderBankAccount.Cash)
            {
                return new ValidationResult("Not Enough Cash For Tax");
            }
            SenderBankAccount.Cash -= cashTotal;
            if (HasAllMerchandiseList == false)
            {
                return new ValidationResult(("Invalid Item detected in gift cart"));
            }
            if (NumberOfItemsAboveRange != SendingGift.MerchandiseTypeId.Count)
            {
                return new ValidationResult(("Not enough property to Gift friends"));

            }
            return ValidationResult.Success;
        }
        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit Task then send 1 else 0.
            return result;
        }
        public void AddTaskTask()
        {

        }

        public void AddResubmitTaskTask(List<string> errorList)
        {
            throw new Exception(string.Join(",", errorList.ToArray()));

        }

    }
}
