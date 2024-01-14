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

    public class CountryTaxServiceController : ApiController
    {

        ICountryTaxDetailsDTORepository _repository;
        IWebUserDTORepository webRepo;
        public CountryTaxServiceController(ICountryTaxDetailsDTORepository repo)
        {
            _repository = repo;
            webRepo = new WebUserDTORepository();
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public CountryTaxServiceController()
        {
            webRepo = new WebUserDTORepository();
            _repository = new CountryTaxDetailsDTORepository();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public CountryTaxDetailsDTO GetCountryTax(string taskId)
        {
            CountryTaxDetailsDTO taxdetails;
            if (taskId == "null")
            {
                int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                string countryid = webRepo.GetCountryId(userid);
                taxdetails = _repository.GetCountryTax(countryid);
            }
            else
            {
                taxdetails = _repository.GetCountryTaxByTask(taskId);
                taxdetails.AllowEdit = false;
            }

            return taxdetails;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public decimal GetGiftTaxRate()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryid = webRepo.GetCountryId(userid);
            decimal taxRate = _repository.GetCountryTaxByCode(countryid,
       AppSettings.TaxGiftCode);
            return taxRate;

        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetRevenueByCountry(string countryId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetRevenueByCountry(countryId);
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

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveTax(CountryTaxDetailsDTO taxDetails)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());

            Task taskA = Task.Factory.StartNew(() => ProcessTax(taxDetails, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Tax Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessTax(CountryTaxDetailsDTO taxDetails, int userid, string countryId)
        {
            try
            {

                string fullName = webRepo.GetFullName(userid);
                taxDetails.AllowEdit = webRepo.isLeader(userid) == "true" ? true : false;
                CountryTaxRules taxrules = new CountryTaxRules(countryId, taxDetails);
                ICountryCodeRepository countryRepo = new CountryCodeRepository();
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = AppSettings.TaxAmendFailNotificationId;
                ValidationResult validationResult = taxrules.IsValid();
                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;
                Guid taskId = Guid.NewGuid();
                CountryCode targetCountry =
                            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryId));

                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveTax(taxDetails, countryId, userid, fullName, taskId);
                    if (!result)
                    {
                        //Add a notification to resubmit 
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
                        notificationTypeId = AppSettings.TaxAmendSuccessNotificationId;

                        using (HttpClient client = new HttpClient())
                        {
                            client.PostAsync(AppSettings.ReminderServiceRunTaskByTypeUrl,
               new StringContent(Http.PutIntoQuotes(AppSettings.TaxTaskType.ToString()),
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

                ExceptionLogging.LogError(ex, "Error to ProcessTax");
            }
        }



    }
}
