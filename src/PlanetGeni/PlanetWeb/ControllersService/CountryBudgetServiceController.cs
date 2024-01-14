using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using PlanetGeni.HttpHelper;
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

    public class CountryBudgetServiceController : ApiController
    {

        ICountryBudgetDetailsDTORepository _repository;
        IWebUserDTORepository webRepo;
        public CountryBudgetServiceController(ICountryBudgetDetailsDTORepository repo)
        {
            _repository = repo;
            webRepo = new WebUserDTORepository();
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public CountryBudgetServiceController()
        {
            _repository = new CountryBudgetDetailsDTORepository();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetCountBudgetPercenTile(string countryId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetCountBudgetPercenTile(countryId);
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

        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public CountryBudgetDetailsDTO GetCountryBudget(string taskId)
        {
            CountryBudgetDetailsDTO budgetypedetails;
            if (taskId == "null")
            {
                string countryid = HttpContext.Current.Session["CountryId"].ToString();
                budgetypedetails =
                    JsonConvert.DeserializeObject<CountryBudgetDetailsDTO>(
                    _repository.GetCountryBudget(countryid));
            }
            else
            {
                budgetypedetails = _repository.GetCountryBudgetByTask(taskId);
                budgetypedetails.AllowEdit = false;
            }

            return budgetypedetails;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public decimal GetCountryBudgetForNationalDefense()
        {

            string countryid = HttpContext.Current.Session["CountryId"].ToString();
            CountryBudgetByType defenseBudget =
                _repository.GetCountryBudgetByType(countryid, AppSettings.NationalBudgetType);
            return defenseBudget.AmountLeft;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveBudget(CountryBudgetDetailsDTO budgetDetails)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => ProcessBudget(budgetDetails, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Budget Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessBudget(CountryBudgetDetailsDTO budgetDetails, int userid, string countryId)
        {
            try
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                string fullName = webRepo.GetFullName(userid);
                ICountryCodeRepository countryRepo = new CountryCodeRepository();
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = AppSettings.BudgetAmendFailNotificationId;

                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;
                Guid taskId = Guid.NewGuid();
                CountryCode targetCountry =
                            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryId));
                CountryBudget budget = _repository.GetCountryBudgetByTaskId(budgetDetails.TaskId);
                budgetDetails.AllowEdit = webRepo.isLeader(userid) == "true" ? true : false;

                CountryBudgetRules budgetrules = new CountryBudgetRules(budgetDetails, budget);
                ValidationResult validationResult = budgetrules.IsValid();

                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveBudget(budgetrules.CheckBeforeSave(), countryId, userid, fullName, taskId);
                    if (!result)
                    {
                        parmText = string.Format("<strong>Date:{0}</strong>|{1}|{2}|{3}",
                       dateTime.ToString(),
                        targetCountry.Code, targetCountry.CountryId,
                        AppSettings.UnexpectedErrorMsg);
                        priority = 8;

                    }
                    else
                    {
                        parmText = string.Format("<strong>Date:{0}</strong>|{1}|{2}",
                    dateTime.ToString(),
                    targetCountry.Code, targetCountry.CountryId);
                        notificationTypeId = AppSettings.BudgetAmendSuccessNotificationId;

                        using (HttpClient client = new HttpClient())
                        {
                            client.PostAsync(AppSettings.ReminderServiceRunTaskByTypeUrl,
               new StringContent(Http.PutIntoQuotes(AppSettings.BudgetTaskType.ToString()),
                   Encoding.UTF8, "application/json"));
                        }
                    }

                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>|{1}|{2}|{3}",
                       dateTime.ToString(),
                        targetCountry.Code, targetCountry.CountryId,
                        validationResult.ErrorMessage);
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
                     notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ProcessBudget");
            }
        }

    }
}
