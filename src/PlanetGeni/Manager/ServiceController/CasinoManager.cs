using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.ServiceController
{
    public class CasinoManager
    {
        ICasinoDTORepository _repository;
        IWebUserDTORepository webRepo;

        private IUserNotificationDetailsDTORepository userNotif;
        private ICountryTaxDetailsDTORepository taxRepo;
        private ICountryCodeRepository countryRepo;
        private IUserBankAccountDTORepository bankRepo;
        public CasinoManager(ICasinoDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            countryRepo = new CountryCodeRepository();
            bankRepo = new UserBankAccountDTORepository();
            taxRepo = new CountryTaxDetailsDTORepository();
        }
        public CasinoManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new CasinoDTORepository();
            countryRepo = new CountryCodeRepository();
            bankRepo = new UserBankAccountDTORepository();
            taxRepo = new CountryTaxDetailsDTORepository();
        }
        public void ProcessRoulete(RouletteDTO roulette)
        {
            try
            {

                if (roulette.SelectedNumber == roulette.WinNumber)
                {
                    roulette.TotalAward = roulette.BetAmount * RulesSettings.RouleteMatchAwardFactor;
                }
                if (roulette.TotalAward == 0)
                {

                    bankRepo.PayNation(
                        new PayNationDTO
                        {
                            Amount = roulette.BetAmount,
                            CountryId = roulette.CountryId,
                            CountryUserId = countryRepo.GetCountryCode(roulette.CountryId).CountryUserId,
                            FundType = AppSettings.LotteryFundType,
                            TaskId = Guid.NewGuid(),
                            Tax = 0,
                            TaxCode = (sbyte)AppSettings.TaxLotteryCode,
                            UserId = roulette.UserId
                        }
                        );
                }
                else
                {
                    if (bankRepo.GetUserBankDetails(roulette.UserId).Cash >= roulette.BetAmount)
                    {
                        decimal taxRatePercent =
                        taxRepo.GetCountryTaxByCode(roulette.CountryId, AppSettings.TaxLotteryCode);
                        bankRepo.PayMe(
                            new PayMeDTO
                            {
                                CountryId = roulette.CountryId,
                                FundType = AppSettings.LotteryFundType,
                                ReciepentId = roulette.UserId,
                                SourceUserId = AppSettings.BankId,
                                TaskId = Guid.NewGuid(),
                                Amount = roulette.TotalAward * (1 - taxRatePercent / 100),
                                Tax = roulette.TotalAward * taxRatePercent / 100,
                                TaxCode = (sbyte)AppSettings.TaxLotteryCode
                            });
                        AddNotificationRoulete(roulette);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessRoulete");
            }
        }
        public void ProcessSlotSpin(SlotNumber slotNumber)
        {
            try
            {
                if (slotNumber.Number1 == slotNumber.Number2 &&
                    slotNumber.Number1 == slotNumber.Number3)
                {
                    slotNumber.TotalAward = slotNumber.BetAmount * RulesSettings.SlotMachineAll3MatchAwardFactor * (slotNumber.Number1 + 1);
                    slotNumber.Match = 3;
                }
                else if (slotNumber.Number1 == slotNumber.Number2 || slotNumber.Number2 == slotNumber.Number3 || slotNumber.Number1 == slotNumber.Number3)
                {
                    slotNumber.TotalAward = slotNumber.BetAmount * RulesSettings.SlotMachine2MatchAwardFactor;
                    slotNumber.Match = 2;
                }

                if (slotNumber.TotalAward == 0)
                {

                    bankRepo.PayNation(
                        new PayNationDTO
                        {
                            Amount = slotNumber.BetAmount,
                            CountryId = slotNumber.CountryId,
                            CountryUserId = countryRepo.GetCountryCode(slotNumber.CountryId).CountryUserId,
                            FundType = AppSettings.LotteryFundType,
                            TaskId = Guid.NewGuid(),
                            Tax = 0,
                            TaxCode = (sbyte)AppSettings.TaxLotteryCode,
                            UserId = slotNumber.UserId
                        }
                        );
                }
                else
                {

                    if (bankRepo.GetUserBankDetails(slotNumber.UserId).Cash >= slotNumber.BetAmount)
                    {
                        decimal taxRatePercent =
                        taxRepo.GetCountryTaxByCode(slotNumber.CountryId, AppSettings.TaxLotteryCode);
                        bankRepo.PayMe(
                            new PayMeDTO
                            {
                                CountryId = slotNumber.CountryId,
                                FundType = AppSettings.LotteryFundType,
                                ReciepentId = slotNumber.UserId,
                                SourceUserId = AppSettings.BankId,
                                TaskId = Guid.NewGuid(),
                                Amount = slotNumber.TotalAward * (1 - taxRatePercent / 100),
                                Tax = slotNumber.TotalAward * taxRatePercent / 100,
                                TaxCode = (sbyte)AppSettings.TaxLotteryCode
                            });
                        AddNotificationSlot(slotNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSlotSpin");
            }
        }
        private void AddNotificationSlot(SlotNumber slotNumber)
        {
            WebUserDTO webUser = webRepo.GetUserPicFName(slotNumber.UserId);
            StringBuilder parmText = new StringBuilder();
            parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                slotNumber.UserId,
                webUser.Picture,
                webUser.FullName,
                slotNumber.TotalAward.ToString("N01"),
                slotNumber.Match
                );

            userNotif.AddNotification(false, string.Empty,
      AppSettings.SlotMachineWinNotificationId, parmText.ToString(), 1, slotNumber.UserId);
        }
        private void AddNotificationRoulete(RouletteDTO roulete)
        {
            WebUserDTO webUser = webRepo.GetUserPicFName(roulete.UserId);
            StringBuilder parmText = new StringBuilder();
            parmText.AppendFormat("{0}|{1}|{2}|{3}",
                roulete.UserId,
                webUser.Picture,
                webUser.FullName,
                roulete.TotalAward.ToString("N01")
                );

            userNotif.AddNotification(false, string.Empty,
      AppSettings.RouleteWinNotificationId, parmText.ToString(), 1, roulete.UserId);
        }
    }
}


