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
    public class PostCommentManager
    {
        IPostCommentDTORepository _repository;
        IWebUserDTORepository webRepo;
        ICountryCodeRepository countryRepo;

        private IUserNotificationDetailsDTORepository userNotif;
        public PostCommentManager(IPostCommentDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            countryRepo = new CountryCodeRepository();
        }
        public PostCommentManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new PostCommentDTORepository();
        }
        public void ProcessBuySpot(BuySpotDTO spotDetails)
        {
            try
            {
                spotDetails.CountryId = webRepo.GetCountryId(spotDetails.UserId);
                spotDetails.CountryUserId = countryRepo.GetCountryCode(spotDetails.CountryId).CountryUserId;


                PostCommentRules spotRule =
                 new PostCommentRules();
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = spotRule.IsValid(spotDetails);
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    _repository.CalculateSpotTotal(ref spotDetails);
                    validationResult = spotRule.IsValidCost(spotDetails);
                }
                if (validationResult == ValidationResult.Success)
                {

                    bool result = _repository.SaveSpot(spotDetails);
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
                        parmText = string.Format("{0}", spotDetails.CalculatedTotalCost); notificationTypeId = AppSettings.AdsSuccessNotificationId;
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
           notificationTypeId, parmText.ToString(), priority, spotDetails.UserId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessAds");
            }
        }

    }
}


