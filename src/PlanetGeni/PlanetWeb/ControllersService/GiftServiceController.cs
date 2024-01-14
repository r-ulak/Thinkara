using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class GiftServiceController : ApiController
    {
        IGiftDTORepository _repository;
        IWebUserDTORepository webRepo = new WebUserDTORepository();
        public GiftServiceController(IGiftDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public GiftServiceController()
        {
            _repository = new GiftDTORepository();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]

        public GiftDeatilsDTO GetGiftDetails()
        {
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            IUserBankAccountDTORepository repoBankAc = new UserBankAccountDTORepository();
            UserBankAccount bankAc = repoBankAc.GetUserBankDetails(userid);
            GiftDeatilsDTO giftdetail = new GiftDeatilsDTO()
            {
                Cash = bankAc.Cash,
                Gold = bankAc.Gold,
                Silver = bankAc.Silver,
                GiftRate = _repository.GetGiftRate(countryId)
            };
            return giftdetail;
        }

        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO SaveSendGiftProperty(GiftDTO giftResponse)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());

            Task taskA = Task.Factory.StartNew(() =>
           ProcessSendGiftProperty(giftResponse, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Gift Successfully Submitted",
                StatusCode = 200
            };
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO SaveSendGift(GiftDTO giftResponse)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() =>
           ProcessSendGift(giftResponse, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Gift Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessSendGift(GiftDTO giftResponse, int userid, string countryId)
        {
            string fullName = webRepo.GetFullName(userid);
            IUserBankAccountDTORepository repoBankAc = new UserBankAccountDTORepository();
            try
            {
                UserBankAccount bankAc = repoBankAc.GetUserBankDetails(userid);
                ICountryTaxDetailsDTORepository countryRepo = new CountryTaxDetailsDTORepository();
                GiftRateDTO giftRate = _repository.GetGiftRate(countryId);
                GiftRules userGiftRule = new GiftRules(
                  giftResponse, bankAc, giftRate);


                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = AppSettings.SendGiftFailNotificationId;
                ValidationResult validationResult = userGiftRule.IsValid();
                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;

                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveSendGift(giftResponse,
                        userGiftRule.SenderBankAccount, userid, giftRate, fullName);
                    if (!result)
                    {
                        parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Capital",
                            dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                        priority = 5;
                    }
                    else
                    {
                        parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Capital",
                            dateTime.ToString());
                        notificationTypeId = AppSettings.SendGiftSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Capital",
                dateTime.ToString(), validationResult.ErrorMessage);
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
                       notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSendGift");
            }
        }

        private void ProcessSendGiftProperty(GiftDTO giftResponse, int userid, string countryId)
        {
            IUserBankAccountDTORepository repoBankAc = new UserBankAccountDTORepository();
            string fullName = webRepo.GetFullName(userid);
            try
            {
                UserBankAccount bankAc = repoBankAc.GetUserBankDetails(userid);
                ICountryTaxDetailsDTORepository countryRepo = new CountryTaxDetailsDTORepository();
                IMerchandiseDetailsDTORepository merchandiseRepo =
                    new MerchandiseDetailsDTORepository();
                decimal total = merchandiseRepo.GetMerchandiseTotal(userid, giftResponse.MerchandiseTypeId.ToArray());
                int itemaboveCount =
                    merchandiseRepo.GetMerchandiseByQty(userid,
                  giftResponse.MerchandiseTypeId.ToArray(),
                  giftResponse.ToId.Count
                  );

                GiftRateDTO giftRate = _repository.GetGiftRate(countryId);
                GiftRules userGiftRule = new GiftRules(
                  giftResponse, bankAc, total, giftRate,
                  merchandiseRepo.HasThisMerchandise(userid,
                  giftResponse.MerchandiseTypeId.ToArray()),
                  itemaboveCount);

                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = AppSettings.SendGiftFailNotificationId;
                ValidationResult validationResult = userGiftRule.IsValidPropertyGift();
                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;


                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveSendPropertyGift(giftResponse,
                        userGiftRule.SenderBankAccount, userid, giftRate, fullName);
                    if (!result)
                    {
                        parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Property",
                            dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                        priority = 5;
                    }
                    else
                    {
                        parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Property",
                            dateTime.ToString());
                        notificationTypeId = AppSettings.SendGiftSuccessNotificationId;
                    }

                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Property",
                dateTime.ToString(), validationResult.ErrorMessage);
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
                       notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSendGiftProperty");
            }
        }
    }
}
