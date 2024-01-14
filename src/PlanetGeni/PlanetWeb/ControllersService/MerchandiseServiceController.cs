using Common;
using DAO;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class MerchandiseServiceController : ApiController
    {

        IMerchandiseDetailsDTORepository _repository;
        public MerchandiseServiceController(IMerchandiseDetailsDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public MerchandiseServiceController()
        {
            _repository = new MerchandiseDetailsDTORepository();
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenPropertyOwner()
        {
            string result = _repository.GetTopTenPropertyOwnerJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]

        public HttpResponseMessage GetMerchandiseTypes(short merchandiseTypeId, sbyte merchandiseTypeCode)
        {
            string result = _repository.GetMerchandiseCodesJson(new MerchandiseCodeSearchDTO
            {
                MerchandiseTypeCode = merchandiseTypeCode,
                MerchandiseTypeId = merchandiseTypeId
            });
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<MerchandiseSummaryDTO> GetMerchandiseSummary()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<MerchandiseSummaryDTO> result = _repository.GetMerchandiseSummaryJson(userid);
            return result;
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public IEnumerable<MerchandiseInventoryDTO> GetMerchandiseInventory(int lastMerchandiseTypeId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<MerchandiseInventoryDTO> merchandiseList =
                _repository.GetMerchandiseInventory(userid, lastMerchandiseTypeId);
            return merchandiseList;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage GetMerchandiseProfile(int profileid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetMerchandiseProfile(profileid);
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
        public PostResponseDTO SaveMerchandiseCart(BuySellMerchandiseDTO[] merchandiseList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => ProcessBuyMerchandiseCart(merchandiseList, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Buy Property Cart Successfully Submitted",
                StatusCode = 200
            };
        }


        public PostResponseDTO SaveSellMerchandiseCart(BuySellMerchandiseDTO[] sellingItems)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => ProcessSellMerchandiseCart(sellingItems, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Sell Property Cart Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessBuyMerchandiseCart(BuySellMerchandiseDTO[] merchandiseList, int userid, string countryId)
        {


            ICountryTaxDetailsDTORepository merchandiseTax = new CountryTaxDetailsDTORepository();
            decimal tax = merchandiseTax.GetCountryTaxByCode(countryId, AppSettings.TaxMerchandiseCode);
            IUserBankAccountDTORepository bankAc = new UserBankAccountDTORepository();
            UserBankAccount buyerBankAccount = bankAc.GetUserBankDetails(userid);

            MerchandiseRules merchandiseCartrules =
            new MerchandiseRules(merchandiseList,
                buyerBankAccount, tax);
            merchandiseCartrules.MerchandiseCodeList = _repository.GetMerchandiseCodesById(merchandiseList.Select(x => x.MerchandiseTypeId).ToArray());
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String parmText = "";
            short notificationTypeId = 0;
            ValidationResult validationResult = merchandiseCartrules.IsValid();
            DateTime dateTime = DateTime.UtcNow;
            sbyte priority = 0;
            if (validationResult == ValidationResult.Success)
            {
                bool result = _repository.SaveMerchandiseCart(
                    merchandiseList, userid, countryId);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.BuySellFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Buy",
                        dateTime.ToString());
                    notificationTypeId = AppSettings.BuySellSuccessNotificationId;
                }


            }
            else
            {
                parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
                dateTime.ToString(), validationResult.ErrorMessage);
                notificationTypeId = AppSettings.BuySellFailNotificationId;
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
                   notificationTypeId, parmText.ToString(), priority, userid);
        }


        private void ProcessSellMerchandiseCart(BuySellMerchandiseDTO[] sellingItems, int userid, string countryId)
        {
            short[] merchandiseIds = sellingItems.Select(x => x.MerchandiseTypeId).ToArray();

            MerchandiseRules merchandiseCartrules =
            new MerchandiseRules(_repository.HasThisMerchandise(userid, merchandiseIds));
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String parmText = "";
            short notificationTypeId = 0;
            ValidationResult validationResult = merchandiseCartrules.IsValidSellCart();
            sbyte priority = 0;
            DateTime dateTime = DateTime.UtcNow;
            if (validationResult == ValidationResult.Success)
            {
                bool result = _repository.SaveSellMerchandiseCart(sellingItems, userid, countryId);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Sell",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.BuySellFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Sell",
                             dateTime.ToString());
                    notificationTypeId = AppSettings.BuySellSuccessNotificationId;
                }

            }
            else
            {
                parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Sell",
                dateTime.ToString(), validationResult.ErrorMessage);
                notificationTypeId = AppSettings.BuySellFailNotificationId;
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
       notificationTypeId, parmText.ToString(), priority, userid);
        }
    }
}