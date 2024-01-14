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
    public class UserTaskServiceController : ApiController
    {
        IUserTaskDetailsDTORepository _repository;
        public UserTaskServiceController(IUserTaskDetailsDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public UserTaskServiceController()
        {
            _repository = new UserTaskDetailsDTORepository();
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public IEnumerable<UserTaskDetailsDTO> GetTaskList(Guid? lastTaskId = null, DateTime? lastCreatedAt = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserTaskDetailsDTO> UserTaskDetailsDTOs =
                _repository.GetTaskList(userid, lastTaskId, lastCreatedAt);
            return UserTaskDetailsDTOs;
        }



    }
}
