using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
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
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]
    public class LotteryServiceController : ApiController
    {
        ILotteryDTORepository _repository;
        public LotteryServiceController(ILotteryDTORepository repo)
        {
            _repository = repo;
        }

        public LotteryServiceController()
        {
            _repository = new LotteryDTORepository();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetNextLotteryDrawingDate()
        {
            string result = _repository.GetNextLotteryDrawingDate();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetPickFiveWinNumber(int lastDrawingId)
        {
            string result = _repository.GetPickFiveWinNumber(lastDrawingId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetPickThreeWinNumber(int lastDrawingId)
        {
            string result = _repository.GetPickThreeWinNumber(lastDrawingId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 5, MustRevalidate = true)]
        public IEnumerable<Pick5WinDTO> GetMyFivePicks
               (int lastDrawingId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<Pick5WinDTO> myFivePicks =
                    _repository.GetMyFivePicks(userid, lastDrawingId);
            return myFivePicks;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 5, MustRevalidate = true)]
        public IEnumerable<Pick3WinDTO> GetMyThreePicks
               (int lastDrawingId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<Pick3WinDTO> myThreePicks =
                    _repository.GetMyThreePicks(userid, lastDrawingId);
            return myThreePicks;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SavePick3Lottery(PickThree pickThree)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => ProcessSavePick3(pickThree, userid));
            return new PostResponseDTO
            {
                Message = "Lottery Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessSavePick3(PickThree pickThree, int userid)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                LotteryRules lotteryRules =
                 new LotteryRules(pickThree);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = lotteryRules.IsValid();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    int result = _repository.SavePick3(userid, pickThree);
                    if (result == 2)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}",
                            "Pick3", AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                        priority = 6;
                    }
                    else if (result == 0)
                    {
                        parmText = string.Format("{0}|{1}",
                             "Pick3", "insufficient fund");
                        notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                    }
                    else
                    {
                        parmText = string.Format("{0}", "Pick3");
                        notificationTypeId = AppSettings.LotteryBuySuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}",
                    "Pick3", validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSavePick3");
            }
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SavePick5Lottery(PickFive pickFive)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => ProcessSavePick5(pickFive, userid));
            return new PostResponseDTO
            {
                Message = "Lottery Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessSavePick5(PickFive pickFive, int userid)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                LotteryRules lotteryRules =
                 new LotteryRules(pickFive);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = lotteryRules.IsValid();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    int result = _repository.SavePick5(userid, pickFive);
                    if (result == 2)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}",
                            "Pick5", AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                        priority = 6;
                    }
                    else if (result == 0)
                    {
                        parmText = string.Format("{0}|{1}",
                             "Pick5", "insufficient fund");
                        notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                    }
                    else
                    {
                        parmText = string.Format("{0}", "Pick5");
                        notificationTypeId = AppSettings.LotteryBuySuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}",
                    "Pick5", validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.LotteryBuyFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSavePick5");
            }
        }
    }
}
