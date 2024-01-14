using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NationalSecurityServiceController : ApiController
    {

        IWeaponDetailsDTORepository _repository;
        public NationalSecurityServiceController(IWeaponDetailsDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public NationalSecurityServiceController()
        {
            _repository = new WeaponDetailsDTORepository();
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenWeaponStackCountry()
        {
            string result = _repository.GetTop10WeaponStackCountryJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetWeaponTypes()
        {
            string result = _repository.GetWeaponCodesJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetSecurityProfile(string countryid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetSecurityProfile(countryid);
                StringContent sc = new StringContent(result);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
                return resp;
            }
            else
            {
                return null;
            }

        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public HttpResponseMessage GetWeaponSummary()
        {
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            string result = _repository.GetWeaponSummaryJson(countryId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public IEnumerable<WeaponInventoryDTO> GetWeaponInventory(int lastWeaponId)
        {
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());

            IEnumerable<WeaponInventoryDTO> weaponList =
                _repository.GetWeaponInventory(countryId, lastWeaponId);
            return weaponList;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveWepaonCart(BuyWeaponDTO[] wepaonList)
        {

            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => ProcessWeaponCart(wepaonList, countryId));
            return new PostResponseDTO
            {
                Message = "Weapon Cart Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessWeaponCart(BuyWeaponDTO[] wepaonList, string countryid)
        {

            string weaponCodesJson = _repository.GetWeaponCodesJson();

            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            NationalSecurityRules weaponCartrules = new NationalSecurityRules(wepaonList, weaponCodesJson, countryid, userid);
            ICountryBudgetDetailsDTORepository countryBudgetType = new CountryBudgetDetailsDTORepository();



            CountryBudgetByType defenseTaskId =
                countryBudgetType.GetCountryBudgetByType(countryid, 1);

            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String parmText = "";
            short notificationTypeId = 0;
            ValidationResult validationResult = weaponCartrules.IsValid();
            sbyte priority = 0;
            DateTime dateTime = DateTime.UtcNow;
            if (validationResult == ValidationResult.Success)
            {

                bool result = _repository.SaveWeaponCart(weaponCartrules.GetCountryWeapons(), defenseTaskId.TaskId);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.WeaponFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Buy",
         dateTime.ToString());
                    notificationTypeId = AppSettings.WeaponSuccessNotificationId;
                }
            }
            else
            {
                parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
                dateTime.ToString(), validationResult.ErrorMessage);
                notificationTypeId = AppSettings.WeaponFailNotificationId;
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
       notificationTypeId, parmText.ToString(), priority, userid);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveWarRequest(WarTargetCountry targetCountry)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());

            Task taskA = Task.Factory.StartNew(() => ProcessWarRequest(targetCountry.TargetCountryId, targetCountry.TargetCountryName, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Request War Key Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessWarRequest(string targetCountryId, string targetCountryName, int userid, string countryId)
        {
            try
            {
                WarRules rules = new WarRules(targetCountryId, targetCountryName, userid, countryId);
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = rules.IsValid();
                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;
                Guid taskId = Guid.NewGuid();

                if (validationResult == ValidationResult.Success)
                {
                    IWarRequestRepository warRequestRepo = new WarRequestRepository();
                    bool result = warRequestRepo.SaveWarRequest(rules.GetWarRequestKey(taskId));
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("<strong>{0}</strong> <span class='flagsprite flagsprite-{1} inline'></span>|<strong>Date:{2}</strong>|{3}",
                            targetCountryName, targetCountryId,
                            dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.WarRequestFailNotificationId;
                        priority = 10;

                    }
                    else
                    {
                        rules.AddAppovalRequestTask(taskId);
                        parmText = string.Format("<strong>{0}</strong> <span class='flagsprite flagsprite-{1} inline'></span>|<strong>Date:{2}</strong>", targetCountryName, targetCountryId,
                                 dateTime.ToString());
                        notificationTypeId = AppSettings.WarRequestSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("<strong>{0}</strong> <span class='flagsprite flagsprite-{1} inline'></span>|<strong>Date:{2}</strong>|{3}",
                        targetCountryName, targetCountryId,
                          dateTime.ToString(), validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.WarRequestFailNotificationId;
                    priority = 10;
                }
                userNotif.AddNotification(false, string.Empty,
                     notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ProcessWarRequest");
            }
        }
    }
}