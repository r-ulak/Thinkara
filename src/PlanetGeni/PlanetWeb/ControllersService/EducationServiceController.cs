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
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class EducationServiceController : ApiController
    {

        IEducationDTORepository _repository;
        public EducationServiceController(IEducationDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public EducationServiceController()
        {
            _repository = new EducationDTORepository();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetDegreeCodes()
        {
            string result = _repository.GetDegreeCodesJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;

            return resp;
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetMajorCodes()
        {

            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetMajorCodesJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<EducationDTO> GetEducationDetails()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<EducationDTO> result = _repository.GetEducationByUserId(userid);
            return result;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetEducationProfile(int profileid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetEducationProfile(profileid);
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
        public IEnumerable<EducationSummaryDTO> GetEducationSummary()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<EducationSummaryDTO> result = _repository.GetEducationSummary(userid);
            return result;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenDegreeHolder()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetTopTenDegreeHolderJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveEnrollDegree(EnrollDegreeDTO[] enrollDegree)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => ProcessSaveEnrollDegree(enrollDegree, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Enroll Degree Successfully Submitted",
                StatusCode = 200
            };
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public EducationBoostDTO ApplyBoostTime(EnrollDegreeDTO enrollDegree)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return _repository.ApplyNextBoost(enrollDegree, userid);
        }
        private void ProcessSaveEnrollDegree(EnrollDegreeDTO[] enrollDegreeList, int userid, string countryId)
        {
            try
            {


                List<MajorCode> majorCodes = JsonConvert.DeserializeObject<List<MajorCode>>(
                  _repository.GetMajorCodesJson());
                ICountryTaxDetailsDTORepository educationTax = new CountryTaxDetailsDTORepository();
                decimal tax = educationTax.GetCountryTaxByCode(countryId, AppSettings.TaxEducationCode);
                IUserBankAccountDTORepository bankAc = new UserBankAccountDTORepository();
                UserBankAccount buyerBankAccount = bankAc.GetUserBankDetails(userid);
                bool meetsPreRequisite = true;// TODO

                EducationRules educationCartrules =
              new EducationRules(enrollDegreeList, majorCodes,
                  buyerBankAccount, meetsPreRequisite, tax);

                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = educationCartrules.IsValid();
                DateTime dateTime = DateTime.UtcNow;
                sbyte priority = 0;
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveEnrollDegree(
                        enrollDegreeList, userid, tax, countryId);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("<strong>Date:{0}</strong>",
                            dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.EducationFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("<strong>Date:{0}</strong>",
                            dateTime.ToString());
                        notificationTypeId = AppSettings.EducationSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>",
                             dateTime.ToString(), validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.EducationFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
                       notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSaveEnrollDegree");
            }


        }
    }
}