using Common;
using DAO.Models;
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
    public class AdsManager
    {
        IAdvertisementDetailsDTORepository _repository;
        IWebUserDTORepository webRepo;

        private IUserNotificationDetailsDTORepository userNotif;
        public AdsManager(IAdvertisementDetailsDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
        }
        public AdsManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new AdvertisementDetailsDTORepository();
        }
        public void ProcessAds(AdvertisementPostDTO adsDetail)
        {
            try
            {
                adsDetail.CountryId = webRepo.GetCountryId(adsDetail.UserId);


                AdvertisementRules adsRule =
                 new AdvertisementRules(adsDetail);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = adsRule.IsValid();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    _repository.CalculateCost(ref adsDetail);
                    validationResult = adsRule.IsValidCost();
                }
                if (validationResult == ValidationResult.Success)
                {

                    bool result = _repository.SaveAds(adsDetail);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}",
AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.AdsFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}", adsDetail.CalculatedTotalCost); notificationTypeId = AppSettings.AdsSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}",
validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.AdsFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, adsDetail.UserId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessAds");
            }
        }

    }
}


