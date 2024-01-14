using DTO.Db;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class AwardServiceController : ApiController
    {
        IAwardDetailsDTORepository _repository;
        public AwardServiceController(IAwardDetailsDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public AwardServiceController()
        {
            _repository = new AwardDetailsDTORepository();
        }


        [HttpGet]
        public IEnumerable<AchievementDTO> GetAchievementDTO(string achievementType)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]); ;
            IEnumerable<AchievementDTO> businessAchivementDTOs = _repository.GetAchievementDTO(userid, achievementType);
            return businessAchivementDTOs;
        }



        [HttpGet]
        public AwardSummaryDTO GetAwardSummaryDTO()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            AwardSummaryDTO awardSummaryDTO = _repository.GetAwardSummaryDTO(userid);
            return awardSummaryDTO;
        }


    }
}
